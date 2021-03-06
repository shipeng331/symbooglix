procedure main();


implementation main()
{
  var x: int;
  var counter: int;
  var bound:int;
  assume bound >=0 && bound < 3;


  anon0:
    assume x > 0;
    counter := 0;
    goto anon2_LoopHead;

  anon2_LoopHead:
    // Note loop body label is first so normal
    // DFS would follow that and we won't escape right away
    goto anon2_LoopBody, anon2_LoopDone;

  anon2_LoopBody:
    assume {:partition} counter < bound;
    counter := counter + 1;
    call x := foo(x);
    goto anon2_LoopHead;

  anon2_LoopDone:
    assume {:partition} counter >= bound;
    return;
}


procedure foo(x: int) returns (r: int);
  requires x > 0;
  ensures r > x;


implementation foo(x: int) returns (r: int)
{

  anon0:
    r := x + 1;
    return;
}


