# Sprint 25 - Aldeia Visual

## Objetivo

Trocar placeholders da Aldeia em Cinzas por arte 2D real (IA/Krita) sem quebrar gameplay da Sprint 24.

## Status: **em andamento** (commit parcial 2026-07-07)

### Entregue

- [x] PNGs em `assets/art/` (usuario: sky, mid, ground, fg)
- [x] Integracao estavel: `aldeia_mid` + `aldeia_fg` via `AldeiaBackground.tscn`
- [x] Parallax horizontal em `AldeiaParallaxBackground.cs`
- [x] Camera Y travada na Aldeia; faixa de andar ajustada (`GetPlayArea` Y=152–208)
- [x] Guias visuais `WalkBandGuide` / `HorizonGuide` na cena
- [x] Remocao de placeholders quando PNG presente
- [x] Documentacao de pipeline (`assets/art/README.md`, handoff Codex)

### Nao entregue / pendente

- [ ] Alinhamento final dos pes (usuario ajusta no editor — ver README)
- [ ] Parallax 4 camadas (exige PNGs com transparencia real)
- [ ] Sprite sheet Arandu idle/walk
- [ ] Capitao / Mata no mesmo estilo visual

## Licoes aprendidas (importante para Codex)

| Tentativa | Resultado |
|-----------|-----------|
| Fundo procedural C# | Revertido — qualidade ruim, parallax bugado |
| Empilhar sky+mid+ground opacos | Um cobre o outro ou tela inteira vira chao |
| Recortar faixas no codigo | Emendas horizontais visiveis |
| Parallax diferente por camada sem assets alinhados | FG “derrapa”, cenario parece colado |
| Numeros magicos no C# | Cada fix quebrava outro lado |

**Abordagem correta:** cena Godot + imagem-mestra (`aldeia_mid`) + FG opcional; codigo so parallax.

## O que NAO fazer de novo

- `AldeiaBackgroundArt` ou poligonos como cenario final
- `RegionRect` automatico em C# para “montar” cenario
- Exigir os 4 PNGs opacos simultaneos no runtime
- Ajustar posicao de sprite via constantes no chat — usar `AldeiaBackground.tscn`

## Registro de validacao visual

- **2026-07-07:** Usuario validou melhora com compositor estavel (mid+fg). Personagem ainda precisava alinhar ao chao — corrigido com faixa Y + cena editavel. Prints em `bugs/`.
- **Pendente:** confirmacao final apos ajuste fino no editor pelo usuario.

## Referencias

- `assets/art/README.md`
- `docs/11_guia_krita_beat_em_up.md`
- `scenes/levels/AldeiaBackground.tscn`
