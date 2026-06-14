import { normalizeName } from "./format";

describe("normalizeName", () => {
  it("apara espaços nas pontas", () => {
    expect(normalizeName("  Acme  ")).toBe("Acme");
  });

  it("colapsa espaços internos", () => {
    expect(normalizeName("Conta   Teste")).toBe("Conta Teste");
  });

  it("trata null e undefined", () => {
    expect(normalizeName(null)).toBe("");
    expect(normalizeName(undefined)).toBe("");
  });
});
