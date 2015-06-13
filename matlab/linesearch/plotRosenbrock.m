close all;

% create a mesh grid
x = linspace(-2,2,50);
y = linspace(-1,3,50);
[X,Y] = meshgrid(x,y);

% evaluate the function
[R, g] = rosenbrock(X, Y);

% add the path to the color maps
addpath(genpath('colormaps'));

%surf(X,Y,R,gradient(R));
surf(X,Y,R,'FaceColor','interp','EdgeAlpha', 0.2);
colormap(haxby);

xlabel('x');
ylabel('y');
zlabel('f(x,y)');
title('Rosenbrock function');

%figure;
%surf(X,Y,g(:,:,1))
%contour(v,v,R,50)
%hold on;
%quiver(v,v,g(:,:,1),g(:,:,2));