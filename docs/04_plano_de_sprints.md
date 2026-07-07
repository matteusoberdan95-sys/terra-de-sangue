# Plano de Sprints

## Regra de ouro

Cada sprint termina com:

- Cena Godot abrindo sem erro.
- Validacao visual no editor ou jogo rodando.
- Checklist marcado.
- Decisao clara: aprovar, ajustar ou cortar.
- Commit local da sprint validada.
- Push para o GitHub.

Nenhuma sprint seguinte comeca sem validacao visual registrada **e** sem commit/push da sprint anterior no remoto.

## Sprint 0 - Fundacao

Objetivo: organizar o projeto e travar a direcao.

Entregas:

- Projeto Godot inicial.
- Documentos de visao, arte, narrativa e gameplay.
- Agentes de validacao definidos.
- Handoff Codex/Cursor documentado.
- Arena prototipo vazia.
- Player e inimigo placeholder.

Status: em andamento.

## Sprint 1 - Combate Prototipo

Objetivo: fazer o combate basico ficar gostoso antes de criar assets finais.

Entregas:

- Movimento 2.5D com colisao.
- Ataque leve com hitbox.
- Inimigo com vida, hit stun e morte.
- Knockback.
- Camera seguindo o player.
- Arena simples com leitura de profundidade.

Criterio de aceite:

- E possivel andar, atacar e matar inimigos.
- Golpe tem resposta visual clara.
- Camera nao atrapalha.
- Profundidade do eixo Y e compreensivel.
- Goku, Vegeta, Krillin e Trunks validam seus checklists.

## Sprint 2 - Vertical Slice Visual

Objetivo: substituir placeholders por uma cena com identidade artistica.

Entregas:

- Sprite inicial do protagonista.
- Sprite inicial do inimigo mercenario.
- Fundo da Aldeia em Cinzas com camadas.
- Particulas de brasa/cinza.
- Paleta aplicada no jogo.

## Sprint 3 - Sistema de Inimigos

Objetivo: transformar bonecos em combate real.

Entregas:

- IA de aproximacao.
- Ataque inimigo.
- Janela de dano.
- Inimigo bruto.
- Spawn por ondas.

## Sprint 4 - Fase 1 Jogavel

Objetivo: montar a primeira fase curta do inicio ao fim.

Entregas:

- Aldeia em Cinzas jogavel.
- 3 encontros.
- 1 mini-chefe.
- Inicio e fim de fase.
- Primeira memoria coletavel.

## Sprint 5 - Polimento de Impacto

Objetivo: deixar o jogo visceral.

Entregas:

- Hit pause.
- Shake de camera.
- Sangue temporario.
- Sons placeholder.
- Feedback de morte.
- Primeiro sistema de feridas/roupa rasgada.
- Primeiros decals de sangue no chao/cenario.

## Sprint 6 - Chefe 1

Objetivo: criar o primeiro chefe memoravel.

Entregas:

- Capitao do Ferro.
- 3 padroes de ataque.
- Intro visual curta.
- Arena propria.
- Recompensa narrativa.

## Sprint 7 - Gore e Execucoes

Objetivo: transformar a brutalidade em diferencial jogavel inicial.

Entregas:

- Sangue persistente no chao.
- Janela de execucao em inimigos quase mortos.
- Feedback de execucao brutal.
- Gore progressivo reforcado na morte.

## Sprint 8 - Kit do Jogador

Objetivo: expandir o combate do protagonista.

Entregas:

- Ataque pesado.
- Combo de 2 golpes no ataque leve.
- Feedback visual diferenciado por tipo de golpe.

## Sprint 9 - Fase 2 Mata Fechada

Objetivo: segunda fase jogavel curta com clima proprio.

Entregas:

- Cena `MataFechada.tscn`.
- Encontros em ambiente de mata escura.
- Transicao apos o chefe 1.

## Sprint 10 - HUD e Progressao

Objetivo: dar leitura de vida, chefe e memorias ao jogador.

Entregas:

- Barra de vida do player.
- Barra de vida do chefe.
- Contador de memorias coletadas.
- Integracao com `MemoryRegistry`.

## Sprint 11 - Gore Avancado

Objetivo: desmembramento, execucoes por arma e sangue em vegetacao.

Entregas:

- Dano visual progressivo avancado.
- Desmembramento por tipo de arma.
- Execucoes contextuais completas.
- Regras de legibilidade reforçadas.

Status: validada em 2026-07-07.

## Sprint 12 - Fase 1 Silhuetas e Identidade

Objetivo: silhuetas 2D definidas, animacao basica e fase 1 polida antes de expandir.

Entregas:

- Silhuetas compostas para player e inimigos da fase 1.
- VisualRig com bob e impulso de ataque.
- Cenario Aldeia em Cinzas com props e brasas pulsantes.
- Memoria e banners mais legiveis.

Status: validada em 2026-07-07.

## Sprint futura - Game Feel Fase 1
