// RUN: %rmdir %t.symbooglix-out
// RUN: %eec 1 %symbooglix --output-dir %t.symbooglix-out --failure-limit=1 %s
// RUN: %ctcy %t.symbooglix-out/termination_counters.yml TerminatedWithoutError 0
// RUN: %ctcy %t.symbooglix-out/termination_counters.yml TerminatedAtFailingAssert 1
procedure main()
{
    var x:int;
    var y:int;

    if (x > 0)
    {
        assert x == 5;
    }
    else
    {
        assert x == -5;
    }

    assert false;
}
