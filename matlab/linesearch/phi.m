function [f] = phi(alpha, fun, x0, direction)
% PHI Calculates ?(?) = f(x0+?*direction)

    p = inputParser;
    addRequired(p, 'alpha', @isscalar);
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    
    parse(p, alpha, fun, x0, direction);

    [f, ~] = fun(x0 + a*direction);
   
end