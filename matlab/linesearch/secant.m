function [c] = secant(a, b, fun, x0, direction)
% SECANT Performs a secant step

    p = inputParser;
    addRequired(p, 'a', @isscalar);
    addRequired(p, 'b', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    
    parse(p, a, b, fun, x0, direction);

    [~, ga] = fun(x0 + a*direction);
    [~, gb] = fun(x0 + b*direction);

    c = (a*(gb'*direction) - b*(ga'*direction)) / ...
        ((gb'*direction) - (ga'*direction));
    
end