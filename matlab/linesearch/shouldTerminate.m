function [ match ] = shouldTerminate( alpha, fun, x0, direction, varargin )
%SHOULDTERMINATE Determines if the inexact line search should terminate

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

    match = originalWolfe(alpha, fun, x0, direction, ...
                          'delta', delta, 'sigma', sigma) ...
            || approximateWolfe(alpha, fun, x0, direction, ...
                          'delta', delta, 'sigma', sigma);    
end

