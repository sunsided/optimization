function [a, b] = bracket(c0, fun, x0, direction, varargin)
% BRACKET Brackets the initial step length

    % expansion factor used in the bracket rule
    defaultRho = 5; % range (0, inf)
    
    % used in the approximate Wolfe termination
    defaultEpsilon = 1E-6; % range [0, inf)
        
    % used in the update rules when the potential intervals [a, c] 
    % or [c, b] violate the opposite slope condition
    defaultTheta = 0.5; % range (0, 1)
    
    p = inputParser;
    addRequired(p, 'c0', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    
    addOptional(p, 'rho', defaultRho, @isscalar);
    addOptional(p, 'epsilon', defaultEpsilon, @isscalar);
    addOptional(p, 'theta', defaultTheta, @isscalar);
        
    parse(p, c0, fun, x0, direction, varargin{:});
    rho = p.Results.rho;
    epsilon = p.Results.epsilon;
    theta = p.Results.theta;
    
    % B0
    j = 0;
    cj = c0;
    
    % initialize previous c-steps
    c = [];

    % also pre-determine the original function value
    [f0, ~] = fun(x0);
    phi0 = f0 + epsilon;
    
    while (true)
    
        % store the c values for backtracking
        c = [cj; c];
        
        % determine the gradient at the currect step length selection
        phiDcj = phiDerived(cj, fun, x0, direction);
        
        % B1
        % by definition, the first step is always a descent direction,
        % so this will never happen in the first loop.
        if phiDcj >= 0
            b = cj;
            
            % find the newest step selection c smaller cj that resulted 
            % in a function value less than the starting point
            for i=2:numel(c)
                if phi(c(i), fun, x0, direction) <= phi0
                    a = c(i);
                    return;
                end
            end

            % error('bracket:b1', 'unable to determine a correct a');
            % This case is not handled in the paper.
            a = 0;
            return;
        
        % B2
        elseif phiDcj < 0 && phi(cj, fun, x0, direction) > phi0
            
            a_bar = 0;
            b_bar = cj;
            
            while (true)

                % U3a
                d = (1-theta)*a_bar + theta*b_bar;

                if phiDerived(d, fun, x0, direction) >= 0
                    a = a_bar;
                    b = d;
                    return
                end

                % U3b
                if phiDerived(d, fun, x0, direction) < 0 && (phi(d, fun, x0, direction) <= phi0)
                    a_bar = d;
                    continue
                end

                % U3c
                b_bar = d;
                continue;

            end

        % B3
        else
            cj = rho*cj;
            continue;
        end
        
    end
    
end