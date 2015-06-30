close all;

%% Prepare the Rosenbrock mesh

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
view(27, 40);
xlim([min(x), max(x)]);
ylim([min(y), max(y)]);
hold on;
colormap(haxby);

xlabel('\theta_1 = x');
ylabel('\theta_2 = y');
zlabel('f(\theta)');
title('Rosenbrock function (a=1, b=100)');

%% Select a starting point

% determine a starting point
startX = -1.5; % datasample(x,1);
startY =  0.6; % datasample(y,1);
[fs, gs] = rosenbrock(startX,startY);

% plot the starting point
plot3(startX, startY, fs, 'r+', 'MarkerSize', 10, 'LineWidth', 2)

% determine the search direction by using the
% inverse direction of the normalized gradient
gradientNorm = norm(gs);
direction = -gs/gradientNorm;
dx = startX+direction(1);
dy = startY+direction(2);

m = 3.5;
endX = startX+m*direction(1); % TODO: determine actual length required
endY = startY+m*direction(2); % TODO: determine actual length required
[fe] = rosenbrock(dx,dy);

% plot the direction point
plot3(dx, dy, fe, 'm+', 'MarkerSize', 5, 'LineWidth', 1)

% construct a plane patch
A = [startX; startY; 4*fs];
B = [startX; startY; 0];
C = [endX; endY; 0];
D = [endX; endY; 4*fs];
PX = [A(1) B(1) C(1) D(1)];
PY = [A(2) B(2) C(2) D(2)];
PZ = [A(3) B(3) C(3) D(3)];
clear A B C D;

patch(PX, PY, PZ, 'r', 'FaceAlpha', 0.25, 'EdgeAlpha', 0.25);
clear PX PY PZ;

%% Sample the line along the direction

% sample the function along the direction for display purposes
lx = linspace(startX, startX+m*direction(1), 50);
ly = linspace(startY, startY+m*direction(2), 50);
lv = linspace(0, m, 50);
lf = rosenbrock(lx, ly);

figure;
h = plot(lv, lf, 'k');

hold on;
title('Rosenbrock function along search direction');
xlabel('\alpha = \nablaf(\theta)/|\nablaf(\theta)|');
ylabel('\phi(\alpha) = f(\theta - \alpha \nablaf(\theta))');

%% Perform a line search

% express the function to optimize in terms of alpha
x = [startX; startY];
alpha = 0;

% plot the initial alpha
a = plot(alpha, fun(x+alpha*direction), '+r');

% do a little inexact 'steepest' descent here
previous_fs = Inf;
value_change_delta = 1E-10;
for i=1:10000

    % determine the local position and gradient
    [fs, gs] = rosenbrock(x(1),x(2));
    
    % abort if no better value can be obtained
    if i > 1 && abs(previous_fs - fs) <= value_change_delta
        disp(['stopping gradient descent in iteration ' num2str(i)]);
        break;
    end
    previous_fs = fs;
    
    % obtain the search direction
    gradientNorm = norm(gs);
    direction = -gs/gradientNorm;
    
    % define the directional function for the line search
    fun = @(theta) rosenbrock(theta(1), theta(2));

    % artificially reset alpha to simulate
    % CG restarts
    if mod(i, 2) == 0
        alpha = 0;
    end
    
    % fire in the hole!
    alpha = hagerZhangLineSearch(fun, x, direction, alpha);
    
    % plottify
    if mod(i, 10) == 0
        % sample the function along the direction for display purposes
        m = 3*alpha;
        lx = linspace(x(1), x(1)+m*direction(1), 50);
        ly = linspace(x(2), x(2)+m*direction(2), 50);
        lv = linspace(0, m, 50);
        lf = rosenbrock(lx, ly);
        set(h,'XData',lv);
        set(h,'YData',lf);
        set(a, 'XData', alpha);
        set(a, 'YData', fun(x+alpha*direction));
        drawnow;
    end
    
    % next minimizer at
    x = x + alpha*direction;
    
    fprintf('location: %+1.4f %+1.4f - direction: %+1.4f %+1.4f\n', x(1), x(2), direction(1), direction(2));
    
end