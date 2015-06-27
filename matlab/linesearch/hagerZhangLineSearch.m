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
    [f0 g0] = fun(x0);

    % initial function values
    f = f0;
    g = g0;
    
    % error tolerance
    epsilon = 1E-5;
    
    % parameters for the Wolfe conditions
    % 0 < delta < sigma < 1
    delta = 0.25;
    sigma = 0.75;
    
    % find an initial bracketing interval [a0,b0]
    alpha = 0;
    
    
    % determine the next point of evaluation
    x_next = x0+alpha*direction;

    % loop variables
    ak = a0;
    bk = b0;
    k = 0;
    while(true)

        % Check for termination

        % T1: Original Wolfe conditions
        [f_next, g_next] = fun(x_next);
        cosine      = g'*direction;
        cosine_next = g_next'*direction;
        if ...
            ((f_next - f) <= (delta*alpha*cosine)) && ... % first Wolfe condition
            (cosine_next >= sigma*cosine)                 % second Wolfe condition
            % original Wolfe conditions are met,
            % so alpha is our final value.
            return;
        end

        % T2: Approximate Wolfe conditions
        if ...
            ((2*delta-1)*g0 >= g_next) & ...
            (g_next >= (sigma * g0))

            % approximate Wolfe conditions are met,
            % so alpha may be a candidate IF there was
            % actually a descent
            if (f_next <= (f0 + epsilon))
                return;
            end
        end

        % L1: perform a double secant step
        [a, b] = secant2(ak, bk);

        % L2: select midpoint
        if (b-a) > (gamma*(bk-ak))
            c = (a+b)/2;
            [a, b] = update(a, b, c);
        end

        % L3: loop
        k = k+1;
        ak = a;
        bk = b;
        alpha = c;

    end
    
    % derp
    alpha = 1;
    
end

