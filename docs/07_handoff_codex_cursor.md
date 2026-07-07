# Handoff Codex e Cursor

Documento para continuar o projeto **sem perder contexto** ao alternar entre Codex, Cursor e o usuario.

## Leitura obrigatoria (ordem)

1. `AGENTS.md`
2. **`docs/08_estado_atual.md`** — onde o projeto esta agora
3. **`docs/sprints/sprint_25_aldeia_visual.md`** — licoes da integracao visual (evitar repetir erros)
4. `docs/04_plano_de_sprints.md`
5. `assets/art/README.md` — como ajustar cenario no Godot
6. `docs/10_politica_validacao_visual.md`

## Onde o projeto esta (2026-07-07)

| Area | Status |
|------|--------|
| Combate / Fase 1 | Jogavel: Aldeia → Capitao → Mata (`Sprint 24` commit `f4e5f52`) |
| Arte Aldeia | `aldeia_mid.png` + `aldeia_fg.png` integrados via `AldeiaBackground.tscn` |
| Personagem | Placeholder poligono / pixel C# — **nao** arte final |
| Cenario Capitao/Mata | Ainda placeholders (poligonos) |

## Dores do usuario (nao ignorar)

1. **Fundos IA opacos** — `aldeia_sky`, `aldeia_mid`, `aldeia_ground` sao cenas completas sem transparencia entre planos. Empilhar ou recortar no codigo **sempre** quebra algo (emendas, chao errado, personagem no ar).
2. **Ajuste no chat/codigo e frustrante** — o usuario quer **montar e alinhar no Godot**, nao ficar pedindo “sobe/desce 10px” no agente.
3. **Expectativa realista** — ficou bonito para um dev solo + IA, mas nao sera AAA sem assets sob medida (camadas transparentes, sprites do personagem, level design no editor).
4. **`aldeia_ground.png`** e vista **de cima** — nao serve para beat 'em up lateral sem regerar.

## Solucao adotada (Sprint 25)

- **Imagem-mestra:** `aldeia_mid.png` (cenario inteiro).
- **Primeiro plano:** `aldeia_fg.png` (centro transparente).
- **Cena editavel:** `scenes/levels/AldeiaBackground.tscn`
  - `AldeiaBackdrop` / `AldeiaForeground` — mover escala no inspector
  - `WalkBandGuide` (laranja) — linha dos pes
  - `HorizonGuide` (azul) — base das cabanas
- **Codigo:** `AldeiaParallaxBackground.cs` aplica **apenas** parallax horizontal (`ScrollFactor` exportado).
- **Arena:** `AldeiaEmCinzasArena.cs` — `LockCameraY`, faixa Y `152–208`, player spawn Y=`180` quando PNG presente.
- **Nao fazer:** `RegionRect` em codigo, empilhar 3 panoramas opacos, fundo procedural C#.

## Arquivos-chave

```text
scenes/levels/AldeiaBackground.tscn   # EDITAR CENARIO AQUI
scenes/levels/AldeiaEmCinzas.tscn     # arena + script
src/Prototype/AldeiaParallaxBackground.cs
src/Prototype/AldeiaEmCinzasArena.cs
src/Prototype/PrototypeArena.cs         # LockCameraY, GetPlayArea()
assets/art/aldeia_mid.png
assets/art/aldeia_fg.png
bugs/                                   # prints de regressao visual
```

## Validacao

```powershell
dotnet build TerraSangrada.csproj
```

Godot:

```text
C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe
```

F5 em `scenes/Main.tscn` — conferir:

- Pes do Arandu na faixa de andar (linha laranja em debug).
- Cabanas **atrás** do personagem, nao no meio do corpo.
- Sem faixa cinza ao mover para cima (camera Y travada na Aldeia).
- Parallax suave ao andar horizontalmente.

## Ao finalizar tarefa

- Atualizar `docs/08_estado_atual.md`
- Decisoes em `docs/09_decisoes.md`
- Commit + push se o usuario pedir ou sprint fechada
