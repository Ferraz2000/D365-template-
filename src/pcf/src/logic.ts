/**
 * Lógica pura do controle PCF (sem ComponentFramework) — testável com Jest.
 * O controle real (init/updateView) delega a estas funções.
 */
export function rotuloEstrelas(nota: number): string {
  const n = Math.max(0, Math.min(5, Math.round(nota)));
  return "★".repeat(n) + "☆".repeat(5 - n);
}
