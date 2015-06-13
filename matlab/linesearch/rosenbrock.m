function [ f, g ] = rosenbrock( x, y, varargin )
%ROSENBROCK Implements the Rosenbrock function

    p = inputParser;
    
    addRequired(p, 'x');
    addRequired(p, 'y');
    addOptional(p, 'a', 1);
    addOptional(p, 'b', 100);

    parse(p,x,y,varargin{:});
    a = p.Results.a;
    b = p.Results.b;
    
    f = (a-x).^2 + b*(y-x.^2).^2;
    
    gx = -2*a + 4*b*x.^3 - 4*b*x.*y + 2*x;
    gy = 2*b*(y-x.^2);
    
    m = size(x,1);
    n = size(x,2);
    
    if n > 1
        g(1:m,1:n,1) = gx;
        g(1:m,1:n,2) = gy;
    elseif m > 1
        g(1:m,1) = gx;
        g(1:m,2) = gy;
    else
        g = [gx; gy];
    end
    
end

