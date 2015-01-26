// RUN: %rmdir %t.symbooglix-out
// RUN: %symbooglix --output-dir %t.symbooglix-out %s --fold-constants=0 --print-instr 2>&1 | %OutputCheck %s
procedure main(p1:int, p2:bv8) returns (r:bv8);

// Bitvector functions
function {:bvbuiltin "bvadd"} bv8add(bv8,bv8) returns(bv8);
function {:bvbuiltin "bvugt"} bv8ugt(bv8,bv8) returns(bool);
function {:bvbuiltin "bvsub"} bv8sub(bv8,bv8) returns(bv8);

var g:bv8;

implementation main(p1:int, p2:bv8) returns (r:bv8)
{
    var a:bv8;
    var b:bv8;
    // CHECK: ${CHECKFILE_ABS_PATH}:${LINE:+1}.*a := 1bv8;
    a := 1bv8;
    // CHECK: ${CHECKFILE_ABS_PATH}:${LINE:+1}.*b := 2bv8;
    b := 2bv8;
    goto BB1, BB2; // This test is fragile, it depends on what state scheduler we use

    BB1:
    // CHECK: ${CHECKFILE_ABS_PATH}:${LINE:+2}.*r := bv8add\(a, b\);
    // CHECK-L: Assignment : r := BVADD8(1bv8, 2bv8)
    r := bv8add(a,b);
    assert bv8ugt(r, 0bv8);
    return;

    BB2:
    // CHECK: ${CHECKFILE_ABS_PATH}:${LINE:+2}.*r := bv8sub\(b, a\);
    // CHECK-L: Assignment : r := BVSUB8(2bv8, 1bv8)
    r := bv8sub(b,a);
    assert r == 1bv8;
    return;
}
