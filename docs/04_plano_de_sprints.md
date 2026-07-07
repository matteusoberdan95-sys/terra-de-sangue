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

## Sprint futura - Gore e Execucoes

Objetivo: transformar a brutalidade em um diferencial real do jogo.

Entregas:

- Dano visual progressivo por inimigo.
- Sangue persistente em chao, vegetacao e objetos.
- Desmembramento por tipo de arma.
- Execucoes brutais: cabeca, barriga, membros e corpo partido.
- Regras de legibilidade para nao esconder o combate.

## Sprint 6 - Chefe 1

Objetivo: criar o primeiro chefe memoravel.

Entregas:

- Capitao do Ferro.
- 3 padroes de ataque.
- Intro visual curta.
- Arena propria.
- Recompensa narrativa.
