function [alpha] = initial(previousAlpha, fun, x0, direction, varargin)
% SECANT Performs a secant step

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
    addRequired(p, 'prviousAlpha', @isscalar);
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
        
        % If x0 = 0, then c = ?0||x0||? / ||g0||? and return.
        elseif abs(max(x0)) == 0
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
        
        %{
        If QuadStep is true, ?(?1?k?1) ? ?(0), and the quadratic 
        interpolant q(·) that matches ?(0), ?(0), and ?(?1?k?1) is 
        strongly convex with a minimizer ?q, then c = ?q and return.
        %}
        
        error('initial:quadstep', 'QuadStep not implemented');
        
    else
       
        alpha = psi2 * previousAlpha;
        
    end
    
end