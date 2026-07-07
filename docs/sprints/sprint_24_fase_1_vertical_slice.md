# Sprint 24 - Fase 1 Vertical Slice

## Objetivo

Jogar **Aldeia em Cinzas → Capitao do Ferro → Mata Fechada** como arco continuo, com progressao e arsenal coerentes.

## Escopo

### Capitao do Ferro

- [x] Sprite pixel dedicado (`IronCaptainSpriteArt`).
- [x] Bleed com duracao reduzida 50% no chefe.
- [x] Telegraph do smash (`IronCrush`) levemente mais longo; investida mais legivel.
- [x] SFX de intro e memoria na vitoria.

### Progressao entre fases

- [x] `WeaponProgression` persiste (tacape T1 apos memoria da Aldeia).
- [x] Capitao: +2 flechas se tacape T1; pickup +2 no chao.
- [x] Mata Fechada: +2 flechas ao entrar; feixe +3 no 1o encontro.

### Mata Fechada

- [x] Spawns de flechas por encontro.
- [x] Pulse de audio no 2o e 3o encontro.
- [x] 3 encontros (12 inimigos no total) com pausas entre ondas.
- [x] Intro e intervalos mais longos para dar peso apos transicao do Capitao.
- [x] Memoria coletavel + tela final do vertical slice (`R` rejoga).

## Testes de validacao

- [x] Completar Aldeia, pegar memoria (no alcance do player), ir ao Capitao com flechas.
- [x] Capitao com sprite proprio; dash/esquiva nos padroes.
- [x] Vitoria leva a Mata Fechada com flechas e encontros.
- [x] Mata Fechada: 3 encontros, memoria final, tela de conclusao.
- [x] Build C# sem erros; validacao visual Godot (editor aberto, F5).

## Correcoes aplicadas (2026-07-07)

- Memoria da Aldeia estava em X=480, fora da faixa jogavel (max X=380).
- Transicao Capitao → Mata via `GameFlow` + `OnDefeated` no chefe.
- Bug entre ondas da Mata (`_active` bloqueava progressao).
- `MemoryPickup` notifica `MataFechadaDirector`.
- Tela final do vertical slice em `GameRoot`.

## Registro de validacao visual

Status atual: **aprovado pelo usuario** em 2026-07-07.

Cena: `scenes/Main.tscn` — arco completo Aldeia → Capitao → Mata → tela final.
