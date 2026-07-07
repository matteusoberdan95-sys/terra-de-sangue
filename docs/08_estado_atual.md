# Estado Atual

Ultima atualizacao: 2026-07-07 (Sprint 25 em andamento — fundo Aldeia integrado, alinhamento fino no editor).

## Sprint atual

**Sprint 25 — Aldeia Visual** (parcialmente entregue)

- PNGs de IA em `assets/art/` (`aldeia_mid`, `aldeia_fg` em uso; `aldeia_sky`, `aldeia_ground` reservados).
- Cenario montado em **`scenes/levels/AldeiaBackground.tscn`** (ajuste visual no Godot, nao no C#).
- Codigo: `AldeiaParallaxBackground.cs` (so parallax horizontal), `AldeiaEmCinzasArena.cs` (câmera Y travada, faixa de andar Y=152–208).
- **Nao** usar fundo procedural em C# (`AldeiaBackgroundArt` foi removido e nao deve voltar).

Ver `docs/sprints/sprint_25_aldeia_visual.md` e `assets/art/README.md`.

## Arco jogavel (Fase 1)

1. **Aldeia em Cinzas** — 3 encontros + Sargento + memoria (Tacape T1)
2. **Capitao do Ferro** — chefe com sprite proprio + flechas
3. **Mata Fechada** — 3 encontros + memoria final + tela de conclusao (`R` rejoga)

## Controles

Ver sprints 20–23 (`J`/`K` corrida, `R` arco, `Shift` dash, etc.).

## Dor recente (ler antes de mexer no cenario)

Tentativas de empilhar/recortar 4 PNGs opacos no **codigo** geraram emendas feias, personagem no ar, regressao a cada fix. **Licao:** posicao/escala do fundo = editor Godot (`AldeiaBackground.tscn`); codigo = parallax + gameplay.

## Proximo passo (ordem sugerida)

1. **Alinhar pes** — abrir `AldeiaBackground.tscn`, mover `AldeiaBackdrop` ate `WalkBandGuide` (F5 + editor).
2. **Sprites Arandu** idle/walk no estilo do `aldeia_mid` (maior impacto visual).
3. (Opcional) Regerar `aldeia_sky` / `aldeia_ground` com **transparencia** para parallax real — ver `assets/art/README.md` Opcao B.
4. Capitao e Mata no mesmo pipeline visual.
