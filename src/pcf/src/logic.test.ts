import { rotuloEstrelas } from "./logic";

describe("rotuloEstrelas", () => {
  it("monta as estrelas conforme a nota", () => {
    expect(rotuloEstrelas(3)).toBe("★★★☆☆");
  });

  it("satura entre 0 e 5", () => {
    expect(rotuloEstrelas(-2)).toBe("☆☆☆☆☆");
    expect(rotuloEstrelas(9)).toBe("★★★★★");
  });
});
