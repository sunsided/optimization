function [a_bar, b_bar] = doubleSecant(a, b, fun, x0, direction, varargin)
% DOUBLESECANT Performs a double secant step.

    % determines when a bisection step is performed
    defaultEpsilon = .66; % range (0, 1)

    % used in the update rules when the potential intervals [a, c] 
    % or [c, b] violate the opposite slope condition
    defaultTheta = .5; % range (0, 1)
    
    p = inputParser;
    addRequired(p, 'a', @isscalar);
    addRequired(p, 'b', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    addOptional(p, 'epsilon', defaultEpsilon, @isscalar);
    addOptional(p, 'theta', defaultTheta, @isscalar);
    
    parse(p, a, b, fun, x0, direction, varargin{:});
    epsilon = p.Results.epsilon;
    theta = p.Results.theta;

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
