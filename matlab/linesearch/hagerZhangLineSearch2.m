function [ alpha ] = hagerZhangLineSearch2( fun, x0, direction, k, varargin )
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

    % determines when a bisection step is performed
    defaultGamma = .66; % range (0, 1)

    p = inputParser;
    addRequired(p, 'fun', @(f) isa(f, 'function_handle'));
    addRequired(p, 'x0', @isnumeric);
    addRequired(p, 'direction', @isnumeric);
    addRequired(p, 'k', @isscalar);
    addOptional(p, 'gamma', defaultGamma, @isscalar);
    
    parse(p, fun, x0, direction, k, varargin{:});
    gamma = p.Results.gamma;
    
    % L0
    c = initial(k);
    [a0, b0] = bracket(c);
    j = 0;
    
    % initialize aj, bj for j = 0
    aj = a0;
    bj = b0;
    
    while (true)
        % L1
        [a, b] = doubleSecant(aj, bj);

        % L2
        if (b-a) > gamma*(bj-aj)
            c = (a+b)/2;
            [a, b] = updateBracketing(a, b, c);
        end

        % L3
        j = j+1;
        aj = a;
        bj = b;
    end
    
end