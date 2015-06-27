function [ alpha ] = hagerZhangLineSearch( x0, fun, direction )
%HAGERZHANGLINESEARCH Implements Hager-Zhang line search.

    %{
    @article{DBLP:journals/siamjo/HagerZ05,
      author    = {William W. Hager and
                   Hongchao Zhang},
      title     = {A New Conjugate Gradient Method with Guaranteed Descent and an Efficient
                   Line Search},
      journal   = {{SIAM} Journal on Optimization},
      volume    = {16},
      number    = {1},
      pages     = {170--192},
      year      = {2005},
      url       = {http://dx.doi.org/10.1137/030601880},
      doi       = {10.1137/030601880},
      timestamp = {Fri, 25 Jun 2010 08:07:11 +0200},
      biburl    = {http://dblp2.uni-trier.de/rec/bib/journals/siamjo/HagerZ05},
      bibsource = {dblp computer science bibliography, http://dblp.org}
    }
    %}

    % fetch the initial point (required for the Wolfe conditions)
    [f0, g0] = fun(x0);

    % initial function values
    f = f0;
    g = g0;
    line_derivative_0 = g0'*direction;
    
    % error tolerance
    epsilon = 1E-5;
    
    % parameters for the Wolfe conditions
    % 0 < delta < sigma < 1
    delta = 0.25;
    sigma = 0.75;
    
    % bisection tuning
    gamma = 0.5; % range (0, 1)
    
    % update rule tuning
    theta = 0.5; % range (0, 1)
    
    % find an initial bracketing interval [a0,b0]
    alpha = 0;
    [a0, b0] = findInitialBracketing(x0, direction, fun, f0);
    
    % If the initial bracketing algorithm is badly configured,
    % this really helps a lot:
    % a0 = 0;

    % loop variables
    ak = a0;
    bk = b0;
    c = 0;
    k = 0;
    while(true)

        % check if a point was generated that satisfied the
        % termination conditions
        for alpha = [ak, bk, c]

            % determine the next point of evaluation
            x_next = x0+alpha*direction;

            % Check for termination

            % T1: Original Wolfe conditions
            [f_next, g_next]     = fun(x_next);
            line_derivative      = g'*direction;
            line_derivative_next = g_next'*direction;
            if ...
                ((f_next - f) <= (delta*alpha*line_derivative)) && ... % first Wolfe condition
                (line_derivative_next >= sigma*line_derivative)        % second Wolfe condition
                % original Wolfe conditions are met,
                % so alpha is our final value.
                return;
            end

            % T2: Approximate Wolfe conditions
            if ...
                ((2*delta-1)*line_derivative_0 >= line_derivative_next) && ...
                (line_derivative_next >= sigma*line_derivative_0)

                % approximate Wolfe conditions are met,
                % so alpha may be a candidate IF there was
                % actually a descent
                if (f_next <= (f0 + epsilon))
                    return;
                end
            end

        end
        
        % L1: perform a double secant step
        [a, b] = doubleSecant(ak, bk, fun, x0, direction, epsilon, theta);

        % L2: select midpoint
        if (b-a) > (gamma*(bk-ak))
            c = (a+b)/2;
            [a, b] = updateBracketing(a, b, c, fun, x0, direction, epsilon, theta);
        end

        % L3: loop
        ak = a;
        bk = b;
        k = k+1;

    end
        
end

function [c] = secant(a, b, fun, x0, direction)
% SECANT Performs a secant step

    [~, ga] = fun(x0 + a*direction);
    [~, gb] = fun(x0 + b*direction);

    c = (a*(gb'*direction) - b*(ga'*direction)) / ...
        ((gb'*direction) - (ga'*direction));
    
end

function [a_bar, b_bar] = doubleSecant(a, b, fun, x0, direction, epsilon, theta)
% DOUBLESECANT Performs a double secant step.

    % S1
    c = secant(a, b, fun, x0, direction);
    [A, B] = updateBracketing(a, b, c, fun, x0, direction, epsilon, theta);

    % S2
    if (c == B)
        c_bar = secant(b, B, fun, x0, direction);
    
    % S3
    elseif (c == A)
        c_bar = secant(a, A, fun, x0, direction);
    end
    
    % S4
    if (c == A) || (c == B)
        [a_bar, b_bar] = updateBracketing(A, B, c_bar, fun, x0, direction, epsilon, theta);
    else
        a_bar = A;
        b_bar = B;
    end
        
end

function [a_bar, b_bar] = updateBracketing(a, b, c, fun, x0, direction, epsilon, theta)
% UPDATEBRACKETING Updates the bracketing interval.

    % some values required for the tests
    % note that they are already available from the context
    % of the calling function, so recalculating them here is inefficient.
    [f0, ~]  = fun(x0);
    [fc, gc] = fun(x0 + c*direction);

    % U0
    if c<a || c>b
        a_bar = a;
        b_bar = b;
        return;
    end
    
    % U1
    if gc'*direction >= 0
        a_bar = a;
        b_bar = c;
        return;
    end

    % U2
    if (gc'*direction < 0) && (fc <= f0 + epsilon)
        a_bar = c;
        b_bar = b;
        return;
    end
    
    % U2
    % if (gc'*direction < 0) && (fc > f0 + epsilon)
    a_hat = a;
    b_hat = c;

    while (true)

        % U2a
        d = (1-theta)*a_hat + theta*b_hat;
        [fd, gd] = fun(x0 + d*direction);

        if gd'*direction >= 0
            a_bar = a_hat;
            b_bar = d;
            return
        end

        % U2b
        if (gd'*direction < 0) && (fd <= f0+epsilon)
            a_hat = d;
            continue
        end

        % U2c
        %if (gd'*direction < 0) && (fd > f0+epsilon)
        b_hat = d;
        continue;

    end
    
end


function [a, b] = findInitialBracketing(x0, direction, fun, f0)
% FINDINITIALBRACKETING Finds an initial bracketing interval [a,b] for the
% line search. Described as the Forward-Backward method in Optimization
% Theory and Methods: Nonlinear Programming by Sun and Yuan.

    %{
    @book{sun2006optimization,
      title={Optimization Theory and Methods: Nonlinear Programming},
      author={Sun, W. and Yuan, Y.X.},
      isbn={9780387249759},
      lccn={2005042696},
      series={Springer Optimization and Its Applications},
      url={https://books.google.de/books?id=nwcenwEACAAJ},
      year={2006},
      publisher={Springer US}
    }
    %}
    
    if ~exist('f0', 'var')
        f0 = fun(x0);
    end

    t     = 1.1; % step length increase factor
    h     = 0.1; % initial step length
    alpha = 0;   % initial alpha
    f     = f0;  % initial function value
    k     = 0;   % iteration counter
    
    % set a number of iterations after which the line search stops
    while (true)

        % evaluate the function at the new step
        alpha_next = alpha + h;
        f_next     = fun(x0 + alpha_next*direction);

        % divide the search interval
        if f_next < f
            
            % perform a forward step
            h_next = t*h;
           
        else
            
            % perform a backward step
            
            % if this is the first iteration, invert the search direction
            if k == 0
                h = -h;
            else
                a = min(alpha, alpha_next);
                b = max(alpha, alpha_next);
                return;
            end
            
        end

        % time update
        f     = f_next;
        alpha = alpha_next;
        h     = h_next;
        k     = k+1;
        
    end
    
end