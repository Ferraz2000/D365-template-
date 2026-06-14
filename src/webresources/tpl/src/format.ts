/**
 * Lógica pura, testável — sem dependência do Xrm.
 * Mantém a regra de normalização do nome igual à do plugin (uma fonte de verdade conceitual).
 */
export function normalizeName(raw: string | null | undefined): string {
  return (raw ?? "").trim().replace(/\s+/g, " ");
}
