import { emailValido, cnpjValido } from "./validacao";

describe("emailValido", () => {
  it("aceita email válido", () => expect(emailValido("ada@contoso.com")).toBe(true));
  it("rejeita inválido/vazio/nulo", () => {
    expect(emailValido("contoso.com")).toBe(false);
    expect(emailValido("")).toBe(false);
    expect(emailValido(null)).toBe(false);
    expect(emailValido(undefined)).toBe(false);
  });
});

describe("cnpjValido", () => {
  it("aceita 14 dígitos (com ou sem máscara)", () => {
    expect(cnpjValido("11.222.333/0001-44")).toBe(true);
    expect(cnpjValido("11222333000144")).toBe(true);
  });
  it("rejeita tamanho errado", () => {
    expect(cnpjValido("123")).toBe(false);
    expect(cnpjValido(null)).toBe(false);
  });
});
