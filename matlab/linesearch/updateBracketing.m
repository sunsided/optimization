function [a_bar, b_bar] = updateBracketing(a, b, c, fun, x0, direction, varargin)
% UPDATEBRACKETING Updates the bracketing interval.

    % determines when a bisection step is performed
    defaultEpsilon = .66; % range (0, 1)

    % used in the update rules when the potential intervals [a, c] 
    % or [c, b] violate the opposite slope condition
    defaultTheta = .5; % range (0, 1)
    
    p = inputParser;
    addRequired(p, 'a', @isscalar);
    addRequired(p, 'b', @isscalar);
    addRequired(p, 'c', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    addOptional(p, 'epsilon', defaultEpsilon, @isscalar);
    addOptional(p, 'theta', defaultTheta, @isscalar);
    
    parse(p, a, b, c, fun, x0, direction, varargin{:});
    epsilon = p.Results.epsilon;
    theta = p.Results.theta;

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
    
    % U3
    % if (gc'*direction < 0) && (fc > f0 + epsilon)
    a_hat = a;
    b_hat = c;

    while (true)

        % U3a
        d = (1-theta)*a_hat + theta*b_hat;
        [fd, gd] = fun(x0 + d*direction);

        if gd'*direction >= 0
            a_bar = a_hat;
            b_bar = d;
            return
        end

        % U3b
        if (gd'*direction < 0) && (fd <= f0+epsilon)
            a_hat = d;
            continue
        end

        % U3c
        %if (gd'*direction < 0) && (fd > f0+epsilon)
        b_hat = d;
        continue;

    end
    
end