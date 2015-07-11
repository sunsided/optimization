function [alpha] = initial(previousAlpha, fun, x0, direction, varargin)
% INITIAL Determines the initial step length

    % The user-defined starting point in the line search
    % or nan if none is given
    defaultStartingPoint = nan;

    % Determines if QuadStep is enabled (if true) or not (if false)
    defaultQSEnabled = false;
    
    % small factor used in starting guess
    defaultPsi0 = .01; % range (0, 1)
    
    % small factor
    defaultPsi1 = .1; % range (0, 1)
    
    % factor multiplying previous step ?(k?1)
    defaultPsi2 = 2; % range (1, ?)
    
    p = inputParser;
    addRequired(p, 'previousAlpha', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    
    addOptional(p, 'startingPoint', defaultStartingPoint, @isscalar);
    addOptional(p, 'quadStepEnabled', defaultQSEnabled, @isscalar);
    addOptional(p, 'psi0', defaultPsi0, @isscalar);
    addOptional(p, 'psi1', defaultPsi1, @isscalar);
    addOptional(p, 'psi2', defaultPsi2, @isscalar);
        
    parse(p, previousAlpha, fun, x0, direction, varargin{:});
    startingPoint = p.Results.startingPoint;
    quadStepEnabled = p.Results.quadStepEnabled;
    psi0 = p.Results.psi0;
    psi1 = p.Results.psi1;
    psi2 = p.Results.psi2;
    
    % helper functions
    supnorm = @(vector) max(abs(vector));
    
    % determine the gradient at the current location
    [f0, g0] = fun(x0);
    
    % I0
    % instead of k we'll use previousAlpha here,
    % since there's no direct dependency on the variable k itself;
    % we do, however, require alpha(k-1) later.
    if previousAlpha == 0
        
        % if the user defined a starting point, use that
        if ~isnan(startingPoint)
            alpha = startingPoint;
        
        % If x0 ~= 0, then c = ?0||x0||? / ||g0||? and return.
        elseif abs(max(x0)) > 0
            alpha = psi0 * supnorm(x0) / supnorm(g0);
            
        % If f (x0) = 0, then c = ?0 |f(x0)| /||g0||2 and return.
        elseif f0 ~= 0 
            alpha = psi0 * abs(f0) / (g0'*g0);
        
        % Otherwise, c = 1 and return.
        else
            alpha = 1;
        end

        return;
    end
    
    % I1
    if quadStepEnabled
        
        % first we check the condition for QuadStep to be allowed
        R = psi1*previousAlpha;
        [f1, ~] = fun(x0+ R*direction);
        if f1 <= f0
            
            % determine the parameters for the quadratic interpolant
            % q(x) = ax^2+bx+c
            % that matches the points phi(0) and phi(psi1*previousAlpha)
            % as well as the derivative phi'(0) at x=0.
            g = g0'*direction;
            
            d = R^2;
            a = - (f0-f1+f1*g)/d;
            %b = - (0^2*g-R^2*g-2*0*f0+2*0*f1)/d;
            %c = (0^2*f1+R^2*f0-2*0*R*f0-0*R^2*g+0^2*R*g)/d;
            
            % find the minimizer
            aq = 0.5 * (R^2*g)/(f0-f1+R*g);
            assert( 0 <= aq );
            
            % one requirement is that the interpolant must
            % be strongly convex. Since that requires
            % the second derivative of q(x) to be greater
            % than zero (or any epsilon), we have
            % q''(x) = 2a > epsilon, requiring a to be
            % positive.
            if a > 1E-5 % && aq > 0
                alpha = aq;                
                return;
            end
            
        end
        
    end
    
    % I2, fallback if no other condition matched
    alpha = psi2 * previousAlpha;
    
end