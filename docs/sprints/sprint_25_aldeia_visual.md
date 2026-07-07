# Sprint 25 - Aldeia Visual

## Objetivo

Trocar placeholders da Aldeia em Cinzas por arte 2D real (IA/Krita) sem quebrar gameplay da Sprint 24.

## Status: **em andamento** (commit parcial 2026-07-07)

### Entregue

- [x] PNGs em `assets/art/` (usuario: sky, mid, ground, fg)
- [x] Integracao estavel: `aldeia_mid` + `aldeia_fg` via `AldeiaBackground.tscn`
- [x] Parallax horizontal em `AldeiaParallaxBackground.cs`
- [x] Camera Y travada na Aldeia; faixa de andar ajustada (`GetPlayArea` Y=152-208)
- [x] Guias visuais `WalkBandGuide` / `HorizonGuide` na cena
- [x] Guias/faixas de debug ocultas no runtime PNG
- [x] Vida ambiente inicial: fogo, fumaca e brasas leves sobre o PNG mestre
- [x] Spritesheets VFX integrados: `aldeia_fire_sheet`, `aldeia_embers_sheet`, `aldeia_smoke_sheet`
- [x] Arandu walk sheet inicial integrado e validado (`assets/art/sprites/player/arandu_walk_sheet.png`)
- [x] Arandu idle vivo, ataque leve e ataque forte inicial integrados (`arandu_idle_sheet`, `arandu_attack_light_sheet`, `arandu_attack_heavy_sheet`)
- [x] Remocao de placeholders quando PNG presente
- [x] Documentacao de pipeline (`assets/art/README.md`, handoff Codex)

### Nao entregue / pendente

- [ ] Alinhamento final dos pes (usuario ajusta no editor - ver README)
- [ ] Parallax 4 camadas (exige PNGs com transparencia real)
- [ ] Sprite sheets Arandu restantes: run, hit/dano, morte
- [ ] Capitao / Mata no mesmo estilo visual

## Receita aprovada pelo usuario

O resultado aprovado veio de uma regra simples: **preservar a arte mestre e animar por cima, sem desmontar o PNG opaco**.

1. `aldeia_mid.png` fica como imagem-mestra opaca da fase.
2. `aldeia_fg.png` fica como foreground transparente.
3. `AldeiaBackground.tscn` controla posicao/escala no editor Godot.
4. `AldeiaParallaxBackground.cs` faz apenas:
   - parallax horizontal;
   - fogo pequeno;
   - fumaca transparente;
   - brasas subindo;
   - spritesheets VFX em `assets/art/vfx/` quando disponiveis;
   - oscilacao sutil do foreground;
   - guias visiveis so no editor.
5. `AldeiaEmCinzasArena.cs` controla gameplay:
   - camera Y travada;
   - faixa jogavel Y=152-208;
   - player em Y=180;
   - sem `BackDepthLimit` / `FrontDepthLimit` no runtime PNG.

Por que funcionou rapido: o codigo deixou de tentar reconstruir uma pintura por recortes. O PNG bonito continua inteiro, e o movimento entra como camada discreta por cima.

## Licoes aprendidas (importante para Codex)

| Tentativa | Resultado |
|-----------|-----------|
| Fundo procedural C# | Revertido - qualidade ruim, parallax bugado |
| Empilhar sky+mid+ground opacos | Um cobre o outro ou tela inteira vira chao |
| Recortar faixas no codigo | Emendas horizontais visiveis |
| Parallax diferente por camada sem assets alinhados | FG derrapa, cenario parece colado |
| Numeros magicos no C# | Cada fix quebrava outro lado |

**Abordagem correta:** cena Godot + imagem-mestra (`aldeia_mid`) + FG opcional; codigo so parallax/vida ambiente leve.

## O que NAO fazer de novo

- `AldeiaBackgroundArt` ou poligonos como cenario final
- `RegionRect` automatico em C# para montar cenario
- Exigir os 4 PNGs opacos simultaneos no runtime
- Ajustar posicao de sprite via constantes no chat - usar `AldeiaBackground.tscn`

## Registro de validacao visual

- **2026-07-07:** Usuario validou melhora com compositor estavel (mid+fg). Personagem ainda precisava alinhar ao chao - corrigido com faixa Y + cena editavel. Prints em `bugs/`.
- **2026-07-07:** Codex validou build e captura local (`outputs/aldeia_validation_game_crop.png`): linhas de debug nao aparecem no runtime, jogador na faixa do chao, efeitos ambientais carregando sem erro.
- **2026-07-07:** VFX spritesheets integrados e validados em `outputs/aldeia_vfx_validation_game_crop.png`: chamas, brasas e fumaca usam PNGs transparentes em `assets/art/vfx/`, sem fundo preto e sem cobrir o combate.
- **2026-07-07:** VFX reancorados ao parallax do fundo apos feedback do usuario. Correcao: `AmbientLife` acompanha o mesmo deslocamento horizontal do `AldeiaBackdrop`, com escala/alpha reduzidos para evitar fogo e fumaca soltos no caminho.
- **2026-07-07:** Arandu walk sheet integrado via `AranduSpriteArt` e validado pelo usuario: personagem apareceu bem no jogo e substitui o bloco no movimento principal.
- **2026-07-07:** Arandu idle vivo e ataque leve integrados em `AranduSpriteArt`; ataque leve recebido com canvas irregular foi normalizado para 8 frames de 256x256 antes da integracao.
- **2026-07-07:** Arandu ataque forte integrado em `AranduSpriteArt`; sheet irregular foi normalizado para 8 frames de 256x256 por silhueta para preservar tacape, bracos e proporcao.
- **2026-07-07:** Validacao local apos ataque forte: `dotnet build TerraSangrada.csproj --no-restore` passou sem erros e Godot foi aberto no projeto/cena principal para checar runtime.
- **Pendente:** confirmacao final apos ajuste fino no editor pelo usuario.

## Referencias

- `assets/art/README.md`
- `docs/11_guia_krita_beat_em_up.md`
- `scenes/levels/AldeiaBackground.tscn`
