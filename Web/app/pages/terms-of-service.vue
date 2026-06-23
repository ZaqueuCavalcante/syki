<script setup lang="ts">
definePageMeta({
  layout: 'landing'
})

useSeoMeta({
  title: 'Termos de Serviço — Estud',
  description: 'Conheça os termos e condições de uso da plataforma Estud.'
})

const lastUpdated = '16 de junho de 2026'
</script>

<template>
  <UContainer class="py-16 max-w-3xl">
    <h1 class="text-3xl font-bold text-gray-900 dark:text-white mb-2">
      Termos de Serviço
    </h1>
    <p class="text-sm text-muted mb-12">Última atualização: {{ lastUpdated }}</p>

    <div class="prose dark:prose-invert max-w-none space-y-10 text-gray-700 dark:text-gray-300">

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">1. Aceitação dos termos</h2>
        <p>
          Ao acessar ou utilizar o <strong>Estud</strong>, você concorda com estes Termos de Serviço.
          Caso não concorde com qualquer parte destes termos, você não deve utilizar a plataforma.
          O uso continuado representa o aceite integral das condições aqui descritas.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">2. Descrição do serviço</h2>
        <p>
          O <strong>Estud</strong> é uma plataforma de gestão acadêmica (SGA) que oferece ferramentas para
          instituições de ensino gerenciarem alunos, professores, cursos, turmas, frequência e notas.
          Os recursos disponíveis podem variar conforme o plano contratado e o perfil de acesso do usuário.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">3. Cadastro e conta</h2>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li>O acesso à plataforma depende de cadastro com informações verídicas e atualizadas.</li>
          <li>Você é responsável por manter a confidencialidade das suas credenciais de acesso.</li>
          <li>Todas as atividades realizadas com a sua conta são de sua responsabilidade.</li>
          <li>Notifique-nos imediatamente em caso de uso não autorizado da sua conta.</li>
        </ul>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">4. Uso aceitável</h2>
        <p>Ao utilizar o Estud, você concorda em <strong>não</strong>:</p>
        <ul class="list-disc pl-6 mt-2 space-y-1">
          <li>Utilizar a plataforma para fins ilegais ou não autorizados.</li>
          <li>Tentar acessar dados, contas ou sistemas de forma indevida.</li>
          <li>Interferir no funcionamento, na segurança ou na integridade do serviço.</li>
          <li>Reproduzir, copiar ou revender qualquer parte da plataforma sem autorização.</li>
        </ul>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">5. Conteúdo e dados</h2>
        <p>
          Os dados acadêmicos inseridos na plataforma pertencem à instituição de ensino e aos seus
          respectivos titulares. O Estud atua como operador desses dados, tratando-os conforme a nossa
          <NuxtLink to="/privacy-policy" class="text-primary hover:underline">Política de Privacidade</NuxtLink>
          e a Lei Geral de Proteção de Dados (LGPD).
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">6. Disponibilidade do serviço</h2>
        <p>
          Empenhamo-nos em manter a plataforma disponível e estável, mas não garantimos funcionamento
          ininterrupto. Manutenções programadas, atualizações ou fatores externos podem causar
          indisponibilidades temporárias.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">7. Limitação de responsabilidade</h2>
        <p>
          O Estud é fornecido "no estado em que se encontra". Na máxima extensão permitida por lei, não nos
          responsabilizamos por danos indiretos, perda de dados ou lucros cessantes decorrentes do uso ou da
          impossibilidade de uso da plataforma.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">8. Suspensão e encerramento</h2>
        <p>
          Podemos suspender ou encerrar o acesso de contas que violem estes Termos de Serviço ou que
          representem risco à segurança da plataforma e dos demais usuários.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">9. Alterações nos termos</h2>
        <p>
          Podemos atualizar estes termos periodicamente. Alterações relevantes serão comunicadas por
          e-mail ou notificação na plataforma. O uso contínuo após a notificação implica aceite das
          mudanças.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">10. Legislação aplicável</h2>
        <p>
          Estes termos são regidos pelas leis da República Federativa do Brasil. Eventuais controvérsias
          serão dirimidas no foro da comarca do domicílio do contratante.
        </p>
      </section>

      <section>
        <h2 class="text-xl font-semibold text-gray-900 dark:text-white mb-3">11. Contato</h2>
        <p>
          Para dúvidas relacionadas a estes Termos de Serviço, entre em contato:
        </p>
        <p class="mt-2">
          E-mail: <a href="mailto:suporte@estud.com.br" class="text-primary hover:underline">suporte@estud.com.br</a>
        </p>
      </section>

    </div>
  </UContainer>
</template>
