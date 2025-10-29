export function load({ url }) {
  const siteTitle = "Syki";
  const siteDescription = "Sistema de Gestão Escolar Open Source";
  const siteImage = "https://svelte.syki.com.br/og_image.png";
  const siteUrl = url.href;

  return { siteTitle, siteDescription, siteImage, siteUrl };
}
