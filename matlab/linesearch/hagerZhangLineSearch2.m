function [ alpha ] = hagerZhangLineSearch2( fun, x0, direction )
%HAGERZHANGLINESEARCH Implements Hager-Zhang line search.

    %{
    @Article{Hager:2006:ACD,
      author =       "William W. Hager and Hongchao Zhang",
      title =        "{Algorithm 851}: {CG}&#095;{DESCENT}, a conjugate gradient
                     method with guaranteed descent",
      journal =      "{ACM} Transactions on Mathematical Software",
      volume =       "32",
      number =       "1",
      pages =        "113--137",
      month =        mar,
      year =         "2006",
      URL =          "http://doi.acm.org/10.1145/1132973.1132979",
      abstract =     "Recently, a new nonlinear conjugate gradient scheme
                     was developed which satisfies the descent condition
                     $g^T_k d_k \leq -7/8 ||g_k||^2$ and which is globally
                     convergent whenever the line search fulfills the Wolfe
                     conditions. This article studies the convergence
                     behavior of the algorithm; extensive numerical tests
                     and comparisons with other methods for large-scale
                     unconstrained optimization are given.",
    }
    %}

    
    
end