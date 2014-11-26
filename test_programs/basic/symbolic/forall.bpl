// RUN: %rmdir %t.symbooglix-out
// RUN: %symbooglix --output-dir %t.symbooglix-out %s 2>&1 | %OutputCheck %s
procedure main()
{
    // CHECK-L: Creating Symbolic ~sb_m_0:[bv8][bv16]bv32
    var m:[bv8][bv16]bv32;
    // CHECK-L: Assume : (forall x: bv8, y: bv16 :: ~sb_m_0[x][y] == 0bv32)
    assume (forall x:bv8, y:bv16 :: m[x][y] == 0bv32);
    // CHECK-L: Assert : (forall a: bv8, b: bv16 :: ~sb_m_0[a][b] == 0bv32)
    assert (forall a:bv8, b:bv16 :: m[a][b] == 0bv32);
}
