export function load({ url }) {
  const siteTitle = "Syki - Sistema de Gestão Escolar Open Source";
  const siteDescription = "Sistema de gestão escolar completo e open source, que pode ser usado por gestores, professores, alunos e pais.";
  const siteImage = "https://svelte.syki.com.br/preview.png";
  const siteUrl = url.href;

  return { siteTitle, siteDescription, siteImage, siteUrl };
}
