# Bugs reportados

Pasta para prints e relatos de bugs durante o desenvolvimento.

## 2026-07-07 — Travamento apos Capitao do Ferro

- **Print:** `image.png`
- **Sintoma:** mata o Capitao, arena vazia, nada avanca.
- **Causa:** vitória do chefe não disparava transição de forma confiável; UI do `BossDirector` estava em coordenadas de mundo (fora da tela); `PhaseDirector` da Aldeia podia coexistir na arena do Capitão.
- **Correção:** `OnDefeated` no chefe chama `BossDirector` diretamente; UI em `CanvasLayer`; arena do Capitão não spawna mais `PhaseDirector`; `GameRoot.DeferredLoadLevel` sem lambda.
