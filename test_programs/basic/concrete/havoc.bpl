// RUN: %rmdir %t.symbooglix-out
// RUN: %eec 1 %symbooglix --output-dir %t.symbooglix-out --print-instr %s 2>&1 | %OutputCheck %s

procedure main(p1:int, p2:bv8) returns (r:bv8);

// Bitvector functions
function {:bvbuiltin "bvadd"} bv8add(bv8,bv8) returns(bv8);
function {:bvbuiltin "bvugt"} bv8ugt(bv8,bv8) returns(bool);

var g:bv8;

implementation main(p1:int, p2:bv8) returns (r:bv8)
{
    var a:bv8;
    var b:bv8;
    // CHECK-L: Assignment : a := 1bv8
    a := 1bv8;
    // CHECK-L: Assignment : b := 2bv8
    b := 2bv8;
    // CHECK-L: ${CHECKFILE_ABS_PATH}:${LINE:+3}: [Cmd] havoc a, b
    // CHECK-NEXT: ~sb_a_1:bv8
    // CHECK-NEXT: ~sb_b_1:bv8
    havoc a,b;
    // CHECK: Assignment : r := BVADD8\(~sb_a_1, ~sb_b_1\)
    r := bv8add(a,b);
    assert bv8ugt(r, 0bv8);
}
