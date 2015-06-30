function [c] = secant(a, b, fun, x0, direction)
% SECANT Performs a secant step

    [~, ga] = fun(x0 + a*direction);
    [~, gb] = fun(x0 + b*direction);

    c = (a*(gb'*direction) - b*(ga'*direction)) / ...
        ((gb'*direction) - (ga'*direction));
    
end