function [a_bar, b_bar] = doubleSecant(a, b, fun, x0, direction, epsilon, theta)
% DOUBLESECANT Performs a double secant step.

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
