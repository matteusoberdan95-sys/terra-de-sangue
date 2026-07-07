# Handoff Codex e Cursor

Documento para continuar o projeto **sem perder contexto** ao alternar entre Codex, Cursor e o usuario.

## Leitura obrigatoria (ordem)

1. `AGENTS.md`
2. **`docs/08_estado_atual.md`** - onde o projeto esta agora
3. **`docs/sprints/sprint_25_aldeia_visual.md`** - licoes da integracao visual
4. `assets/art/README.md` - receita do cenario da Aldeia
5. `docs/04_plano_de_sprints.md`
6. `docs/10_politica_validacao_visual.md`

## Onde o projeto esta (2026-07-07)

| Area | Status |
|------|--------|
| Combate / Fase 1 | Jogavel: Aldeia -> Capitao -> Mata |
| Arte Aldeia | `aldeia_mid.png` + `aldeia_fg.png` integrados via `AldeiaBackground.tscn` |
| Vida ambiente Aldeia | Fogo, fumaca, brasas e leve oscilacao de foreground em `AldeiaParallaxBackground.cs` |
| Personagem | Placeholder poligono / pixel C# - **nao** arte final |
| Cenario Capitao/Mata | Ainda placeholders (poligonos) |

## Regra de ouro da Aldeia

**Nao remontar a Aldeia empilhando varios PNGs opacos.**

O que funcionou e deve virar padrao:

1. Usar **um PNG mestre opaco bonito** para o cenario inteiro: `assets/art/aldeia_mid.png`.
2. Usar **um foreground transparente** para bordas/troncos/vinhas: `assets/art/aldeia_fg.png`.
3. Posicionar e escalar esses sprites na cena Godot: `scenes/levels/AldeiaBackground.tscn`.
4. Deixar o C# fazer so comportamento leve: parallax horizontal, fogo, fumaca, brasas, pequena oscilacao.
5. Manter gameplay separado: faixa de andar e camera ficam em `AldeiaEmCinzasArena.cs`.

Esse padrao deixou o cenario bonito com poucos assets porque preserva a pintura original do PNG mestre. As tentativas anteriores quebravam porque recortavam ou sobrepunham imagens opacas que nao foram exportadas como camadas reais.

## Como foi feito

### Cena

Arquivo: `scenes/levels/AldeiaBackground.tscn`

- `AldeiaBackdrop`: sprite com `aldeia_mid.png`, a imagem-mestra da fase.
- `AldeiaForeground`: sprite com `aldeia_fg.png`, com centro transparente.
- `WalkBandGuide`: guia dos pes para alinhar no editor.
- `HorizonGuide`: guia da base das cabanas.

As guias existem para edicao no Godot. Elas **nao devem aparecer durante o jogo**.

### Script de fundo

Arquivo: `src/Prototype/AldeiaParallaxBackground.cs`

- Aplica parallax horizontal no backdrop/foreground.
- Cria overlays leves por codigo:
  - chamas pequenas em janelas/ruinas;
  - fumaca cinza transparente;
  - brasas subindo;
  - leve movimento no foreground.
- Mostra guias apenas no editor (`Engine.IsEditorHint()`), nao em runtime.

### Arena

Arquivo: `src/Prototype/AldeiaEmCinzasArena.cs`

- Remove placeholders quando `aldeia_mid.png` existe.
- Instancia `AldeiaBackground.tscn`.
- Trava camera Y na Aldeia.
- Usa faixa jogavel Y=152-208.
- Posiciona Arandu em Y=180.
- Nao cria `BackDepthLimit` / `FrontDepthLimit` quando o PNG esta em uso, para evitar linhas atravessando a fase.

## O que NAO fazer

- Nao recriar `AldeiaBackgroundArt` procedural em C#.
- Nao usar `RegionRect` automatico para recortar `sky`, `mid` e `ground`.
- Nao empilhar `aldeia_sky.png`, `aldeia_mid.png` e `aldeia_ground.png` se forem opacos.
- Nao mover chao/personagem/camera por numeros aleatorios no chat.
- Nao deixar linhas de guia visiveis no jogo rodando.
- Nao usar `aldeia_ground.png` atual como chao lateral: ele esta em vista de cima e nao encaixa no beat 'em up.

## Quando quiser parallax de 4 camadas

So fazer depois que os assets forem exportados como camadas transparentes reais:

| Arquivo | Conteudo | Resto do PNG |
|---------|----------|--------------|
| `aldeia_sky.png` | Ceu, nuvens, fumaca distante | Transparente |
| `aldeia_mid.png` | Cabanas, arvores, fogo | Transparente fora do plano |
| `aldeia_ground.png` | Chao lateral da faixa jogavel | Transparente |
| `aldeia_fg.png` | Primeiro plano nas bordas | Centro transparente |

Se a camada for opaca, ela nao e camada: e outro fundo inteiro. Nao usar no runtime junto com outros fundos inteiros.

## Arquivos-chave

```text
scenes/levels/AldeiaBackground.tscn   # EDITAR CENARIO AQUI
scenes/levels/AldeiaEmCinzas.tscn     # arena + script
src/Prototype/AldeiaParallaxBackground.cs
src/Prototype/AldeiaEmCinzasArena.cs
assets/art/aldeia_mid.png
assets/art/aldeia_fg.png
assets/art/README.md
docs/sprints/sprint_25_aldeia_visual.md
```

## Validacao obrigatoria

```powershell
dotnet build TerraSangrada.csproj --no-restore
```

Abrir Godot:

```powershell
Start-Process -FilePath 'C:\Users\mober\OneDrive\Desktop\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64\Godot_v4.7-stable_mono_win64.exe' -ArgumentList '--path','C:\Users\mober\OneDrive\Desktop\fauna-do-sangue'
```

F5 em `scenes/Main.tscn` e conferir:

- Sem linhas azul/vermelha/marrom atravessando a fase.
- Pes do Arandu na faixa de chao.
- Cabanas atras do personagem, nao cortando o corpo.
- Chamas/fumaca/brasas visiveis, mas sem poluir combate.
- Camera nao sobe para mostrar ceu vazio.

## Ao finalizar tarefa

- Atualizar `docs/08_estado_atual.md`.
- Atualizar `docs/sprints/sprint_25_aldeia_visual.md`.
- Registrar decisao em `docs/09_decisoes.md` se mudar o padrao.
- Commit + push apenas depois da validacao visual quando a mudanca afetar cena/arte/gameplay.
