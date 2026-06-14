/** Validações puras de formulário (sem Xrm) — fáceis de testar. */
export function emailValido(email: string | null | undefined): boolean {
  if (!email) return false;
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}

export function cnpjValido(cnpj: string | null | undefined): boolean {
  if (!cnpj) return false;
  const so = cnpj.replace(/\D/g, "");
  return so.length === 14;
}
