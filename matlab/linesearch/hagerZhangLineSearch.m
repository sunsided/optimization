function [ alpha ] = hagerZhangLineSearch( x0, fun, direction )
%HAGERZHANGLINESEARCH Implements Hager-Zhang line search.

%{
@article{DBLP:journals/siamjo/HagerZ05,
  author    = {William W. Hager and
               Hongchao Zhang},
  title     = {A New Conjugate Gradient Method with Guaranteed Descent and an Efficient
               Line Search},
  journal   = {{SIAM} Journal on Optimization},
  volume    = {16},
  number    = {1},
  pages     = {170--192},
  year      = {2005},
  url       = {http://dx.doi.org/10.1137/030601880},
  doi       = {10.1137/030601880},
  timestamp = {Fri, 25 Jun 2010 08:07:11 +0200},
  biburl    = {http://dblp2.uni-trier.de/rec/bib/journals/siamjo/HagerZ05},
  bibsource = {dblp computer science bibliography, http://dblp.org}
}
%}

    % fetch the initial point (required for the Wolfe conditions)
    y0 = fun(x0);

    % initial alpha
    alpha = 0;
    
    % parameters for the Wolfe conditions
    % 0 < delta < sigma < 1
    delta = 0.25;
    sigma = 0.75;
        
    % Check for termination

    % T1: Original Wolfe conditions
    x_next = x0+alpha*direction;
    [f_next g_next] = fun(x_next);
    cosine = g_next'*direction;
%    if ...
%        ((f_alpha - y0) <= (delta*alpha*cosine)) & ...
%        (true) 
%    end
    
    % T2: Approximate Wolfe conditions

    alpha = 1;
    
end

