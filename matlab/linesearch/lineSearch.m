close all;

% create a mesh grid
x = linspace(-2,2,50);
y = linspace(-1,3,50);
[X,Y] = meshgrid(x,y);

% evaluate the function
[R, g] = rosenbrock(X, Y);

% add the path to the color maps
addpath(genpath('colormaps'));

% plot the surface
surf(X,Y,R,'FaceColor','interp','EdgeAlpha', 0.2);
xlim([min(x), max(x)]);
ylim([min(y), max(y)]);
hold on;
colormap(haxby);

xlabel('\theta_1 = x');
ylabel('\theta_2 = y');
zlabel('f(\theta)');
title('Rosenbrock function (a=1, b=100)');

% determine a starting point
sx = -1.5; % datasample(x,1);
sy =  0.6; % datasample(y,1);
[fs, gs] = rosenbrock(sx,sy);

% plot the starting point
plot3(sx, sy, fs, 'r+', 'MarkerSize', 10, 'LineWidth', 2)

% determine the search direction by using the
% inverse direction of the normalized gradient
d = -gs/sqrt(gs'*gs);
dx = sx+d(1);
dy = sy+d(2);

m = 3.5;
ex = sx+m*d(1); % determine actual length required
ey = sy+m*d(2); % determine actual length required
[fe] = rosenbrock(dx,dy);

% plot the direction point
plot3(dx, dy, fe, 'm+', 'MarkerSize', 5, 'LineWidth', 1)

% construct a plane patch
A = [sx; sy; 4*fs];
B = [sx; sy; 0];
C = [ex; ey; 0];
D = [ex; ey; 4*fs];
PX = [A(1) B(1) C(1) D(1)];
PY = [A(2) B(2) C(2) D(2)];
PZ = [A(3) B(3) C(3) D(3)];
clear A B C D;

patch(PX, PY, PZ, 'r', 'FaceAlpha', 0.25, 'EdgeAlpha', 0.25);
clear PX PY PZ;

% sample the function along the direction for display purposes
lx = linspace(sx, ex, 50);
ly = linspace(sy, ey, 50);
lv = linspace(0, m, 50);
lf = rosenbrock(lx, ly);
figure;
plot(lv, lf);
title('Rosenbrock function along search direction');
xlabel('\lambda = \nablaf(\theta)/|\nablaf(\theta)|');
ylabel('f(\theta - \lambda \nablaf(\theta))');