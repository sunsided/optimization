function [ match ] = originalWolfe( alpha, fun, x0, direction, varargin )
%WOLFECONDITIONS Determines if the original Wolfe conditions are fulfilled

    % used in the Wolfe conditions
    defaultDelta = .1; % range (0, 0.5)
    
    % used in the Wolfe conditions
    defaultSigma = .9; % range [delta, 1)

    p = inputParser;
    addRequired(p, 'alpha', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    addOptional(p, 'delta', defaultDelta, @isscalar);
    addOptional(p, 'sigma', defaultSigma, @isscalar);
    
    parse(p, alpha, fun, x0, direction, varargin{:});
    delta = p.Results.delta;
    sigma = p.Results.sigma;
    
    % determine current values
    [f, g] = fun(x0);
    
    % determine the next point of evaluation
    x_next = x0+alpha*direction;
        
    % Original Wolfe conditions
    [f_next, g_next]     = fun(x_next);
    directional_derivative      = g'*direction;
    directional_derivative_next = g_next'*direction;
    
    match = ...
        ((f_next - f) <= (delta*alpha*directional_derivative)) && ...  % sufficient decrease condition
        (directional_derivative_next >= sigma*directional_derivative); % curvature condition
    
end

