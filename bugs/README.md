# Bugs reportados

Pasta para prints e relatos de bugs durante o desenvolvimento.

## 2026-07-07 — Sprint 25: integracao visual Aldeia

- **Prints:** `image.png` (estado mais recente apos compositor estavel)
- **Sintomas anteriores:** emendas horizontais entre camadas; chao cobrindo tela; personagem no ar / no meio das cabanas; faixa cinza ao subir.
- **Causa:** tentativa de empilhar/recortar PNGs opacos no codigo; camera seguia Y; assets IA nao sao camadas com transparencia.
- **Correcao:** `AldeiaBackground.tscn` editavel no Godot; `aldeia_mid` + `aldeia_fg`; camera Y travada; faixa de andar Y=152–208; ver `docs/sprints/sprint_25_aldeia_visual.md`.
- **Pendente:** alinhamento fino dos pes pelo usuario no editor (`WalkBandGuide`).

## 2026-07-07 — Travamento apos Capitao do Ferro

- **Sintoma:** mata o Capitao, arena vazia, nada avanca.
- **Causa:** vitoria do chefe nao disparava transicao de forma confiavel.
- **Correcao:** Sprint 24 (`f4e5f52`).
