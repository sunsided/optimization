close all;
clear all;

% determine the quadratic interpolant
syms a b c x_0 x_1 q(x_0) q(x_1) dq(x_0)
[a, b, c] = solve(...
      q(x_0) == a*x_0^2 + b*x_0 + c, ...
      q(x_1) == a*x_1^2 + b*x_1 + c, ...
      dq(x_0) == 2*a*x_0 + b, ...
      a, b, c);
  
disp('Solution for a = '); pretty(a);
disp('Solution for c = '); pretty(b);
disp('Solution for b = '); pretty(c);

syms q(x)
q(x) = a*x^2 + b*x + c;
q(x) = simplify(q(x));
disp('Solution for q(x) = '); pretty(q);

%% define the original function
a = -5;
b = 2;
c = 3;
d = 0.5;

fun  = @(x) a*x.^3 + b*x.^2 + c*x + d;
dfun = @(x) 3*a*x.^2 + 2*b*x + c;

%% determine the quadratic interpolant
x0 = -0.8;
x1 = 0.1;

% substitute variables
q = subs(q, ...
    {'q(x_0)', 'q(x_1)', 'dq(x_0)', 'x_0', 'x_1'}, ...
    {fun(x0), fun(x1), dfun(x0), x0, x1});

% determine first derivative
dq = diff(q);

% convert symbolic function to a function handle for easier evaluation
q = matlabFunction(q);
dq = matlabFunction(dq);

% sample the original function
x  = linspace(-1, 1, 50);
y  = fun(x);
yd = dfun(x);

% sample the interpolant
iy  = q(x);
iyd = dq(x);

%% Plottify
figure('Name', 'Quadratic Interpolation Test');

% plot f(x)
subplot(1,2,1);
plot(x, y, ...
    'LineWidth', 2, ...
    'Color', [0 0 .5] ...
    );
hold on;
plot(x, iy, ...
    'LineWidth', 2, ...
    'Color', [1 .5 .5] ...
    );

plot([x0 x1], [fun(x0), fun(x1)], 'o', ...
    'MarkerSize', 5, ...
    'Color', [1 .1 .1] ...
    );

xlabel('x');
ylabel('f(x), q(x)');
grid on;
title('quadratic approximation q(\cdot) \approx f(\cdot)');
legend(...
    'f(x)', ...
    'q(x) \approx f(x)', ...
    'support points', ...
    'Location', 'NorthWest');

% plot f'(x)
subplot(1,2,2);
plot(x, yd, ...
    'LineWidth', 2, ...
    'Color', [0 0 .5] ...
    );
hold on;
plot(x, iyd, ...
    'LineWidth', 2, ...
    'Color', [1 .5 .5] ...
    );

plot(x0, dfun(x0), 'o', ...
    'MarkerSize', 5, ...
    'Color', [1 .1 .1] ...
    );

xlabel('x');
ylabel('f''(x), q''(x)');
grid on;
title('derivative of f(\cdot) and g(\cdot)');
legend(...
    'f''(x)', ...
    'q''(x) \approx f''(x)', ...
    'support point', ...
    'Location', 'NorthWest');