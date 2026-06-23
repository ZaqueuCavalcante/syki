<script setup lang="ts">
definePageMeta({
  layout: 'landing'
})

useSeoMeta({
  title: 'Política de Privacidade — Estud',
  description: 'Saiba como o Estud coleta, usa e protege seus dados pessoais.'
})

const lastUpdated = '16 de junho de 2026'
</script>

<template>
  <UContainer class="py-16 max-w-3xl">
    <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-2">
      Política de Privacidade
    </h1>
    <p class="text-sm text-muted mb-12">Última atualização: {{ lastUpdated }}</p>

    <div class="prose dark:prose-invert max-w-none space-y-10 text-gray-700 dark:text-gray-300">

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">1. Quem somos</h2>
        <p>
          O <strong>Estud</strong> é uma plataforma de gestão acadêmica que oferece ferramentas para
          instituições de ensino gerenciarem alunos, professores, cursos e turmas. Esta Política de
          Privacidade explica como coletamos, usamos, armazenamos e protegemos seus dados pessoais,
          em conformidade com a <strong>Lei Geral de Proteção de Dados (LGPD — Lei nº 13.709/2018)</strong>.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">2. Dados que coletamos</h2>
        <p>Coletamos apenas os dados necessários para o funcionamento da plataforma:</p>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li><strong>Dados de identificação:</strong> nome completo e endereço de e-mail.</li>
          <li><strong>Dados de acesso:</strong> credenciais de login (senha armazenada com hash seguro) e provedores de autenticação social (ex: Google).</li>
          <li><strong>Dados acadêmicos:</strong> matrícula, cursos, turmas, frequência e notas — fornecidos pela instituição à qual você pertence.</li>
          <li><strong>Dados de uso:</strong> logs de acesso, endereço IP e informações do dispositivo, usados exclusivamente para segurança e diagnóstico.</li>
        </ul>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">3. Como usamos seus dados</h2>
        <p>Seus dados são usados para:</p>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li>Autenticar e gerenciar o acesso à plataforma.</li>
          <li>Exibir informações acadêmicas relevantes ao seu perfil (aluno, professor ou gestor).</li>
          <li>Enviar notificações e comunicados relacionados à sua instituição.</li>
          <li>Garantir a segurança da conta (detecção de acessos suspeitos, autenticação de dois fatores).</li>
          <li>Cumprir obrigações legais e contratuais.</li>
        </ul>
        <p class="mt-3">Não vendemos, alugamos nem compartilhamos seus dados com terceiros para fins comerciais.</p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">4. Base legal para o tratamento</h2>
        <p>
          O tratamento dos seus dados é baseado no <strong>cumprimento de contrato</strong> (prestação do serviço à
          instituição) e no <strong>legítimo interesse</strong> para segurança da plataforma, conforme o art. 7º
          da LGPD.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">5. Compartilhamento de dados</h2>
        <p>Seus dados podem ser acessados por:</p>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li><strong>Sua instituição de ensino:</strong> gestores e coordenadores têm acesso aos dados acadêmicos dos usuários da instituição.</li>
          <li><strong>Provedores de infraestrutura:</strong> serviços de hospedagem e banco de dados usados pela plataforma, contratualmente obrigados a manter sigilo.</li>
          <li><strong>Autoridades competentes:</strong> quando exigido por lei ou ordem judicial.</li>
        </ul>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">6. Retenção de dados</h2>
        <p>
          Mantemos seus dados enquanto sua conta estiver ativa ou enquanto a instituição mantiver contrato
          ativo com o Estud. Após o encerramento, os dados são anonimizados ou excluídos em até
          <strong>90 dias</strong>, salvo obrigação legal que exija prazo maior.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">7. Seus direitos (LGPD)</h2>
        <p>Você tem direito a:</p>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li>Confirmar a existência e acessar seus dados tratados por nós.</li>
          <li>Corrigir dados incompletos, inexatos ou desatualizados.</li>
          <li>Solicitar a exclusão dos dados desnecessários ou tratados em desconformidade com a LGPD.</li>
          <li>Portabilidade dos seus dados a outro fornecedor.</li>
          <li>Revogar o consentimento, quando aplicável.</li>
        </ul>
        <p class="mt-3">
          Para exercer esses direitos, entre em contato pelo e-mail
          <a href="mailto:suporte@estud.com.br" class="text-primary hover:underline">suporte@estud.com.br</a>.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">8. Segurança</h2>
        <p>
          Adotamos medidas técnicas e organizacionais para proteger seus dados contra acesso não autorizado,
          perda ou vazamento, incluindo criptografia em trânsito (TLS), senhas com hash seguro (bcrypt),
          autenticação de dois fatores e controle de acesso por perfil.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">9. Cookies</h2>
        <p>
          Usamos cookies de sessão estritamente necessários para manter você autenticado. Não utilizamos
          cookies de rastreamento ou publicidade.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">10. Alterações nesta política</h2>
        <p>
          Podemos atualizar esta política periodicamente. Alterações relevantes serão comunicadas por
          e-mail ou notificação na plataforma. O uso contínuo após a notificação implica aceite das
          mudanças.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">11. Contato</h2>
        <p>
          Para dúvidas ou solicitações relacionadas a esta política, entre em contato com nosso
          Encarregado de Proteção de Dados (DPO):
        </p>
        <p class="mt-2">
          E-mail: <a href="mailto:suporte@estud.com.br" class="text-primary hover:underline">suporte@estud.com.br</a>
        </p>
      </section>

    </div>
  </UContainer>
</template>
