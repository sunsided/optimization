f = @(x,a,b) a*x.^2 + b*x;

x = linspace(-1, 1, 50);

figure;
subplot(3,1,1);
plot(x, f(x,1,1), 'r');
hold;
plot(x, f(x,0,1), 'g');
plot(x, f(x,-1,1), 'b');
legend('a>0', 'a=0', 'a<0', ...
    'Location', 'NorthWest');
xlabel('x');
ylabel('ax^2+bx');

subplot(3,1,2);
plot(x, f(x,1,0), 'r');
hold;
plot(x, f(x,0,0), 'g');
plot(x, f(x,-1,0), 'b');
legend('a>0', 'a=0', 'a<0', ...
    'Location', 'North');
xlabel('x');
ylabel('ax^2');

subplot(3,1,3);
plot(x, f(x,1,-1), 'r');
hold;
plot(x, f(x,0,-1), 'g');
plot(x, f(x,-1,-1), 'b');
legend('a>0', 'a=0', 'a<0', ...
    'Location', 'NorthEast');
xlabel('x');
ylabel('ax^2-bx');