function [ alpha ] = hagerZhangLineSearch( fun )
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

    % First termination criterion: Original Wolfe conditions
    % Second termination criteron: Approximate Wolfe conditions

    alpha = 1;
    
end

