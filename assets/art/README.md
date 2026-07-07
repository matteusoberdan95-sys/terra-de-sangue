# Arte — Terra Sangrada

## Ajuste visual no Godot (sem codigo)

1. Abra **`scenes/levels/AldeiaBackground.tscn`** no editor.
2. Rode o jogo (F5) e volte ao editor, ou abra **`AldeiaEmCinzas.tscn`** em paralelo.
3. Mova **`AldeiaBackdrop`** (cenario) ate a linha laranja **WalkBandGuide** coincidir com os pes do Arandu.
4. Linha azul **HorizonGuide** = base das cabanas (personagem fica abaixo dela).
5. Ajuste **`AldeiaForeground`** se precisar. Salve a cena (Ctrl+S).

O codigo so aplica parallax horizontal — posicao e escala ficam na cena.

## Abordagem correta (importante)

**Nao** empilhar no codigo varios PNGs opacos da mesma cena (sky + mid + ground).
Cada arquivo opaco cobre o anterior ou exige recortes — isso gera emendas feias e bugs
que corrigem um lado e quebram o outro.

### Opcao A — Recomendada agora (Sprint 25)

1. **`aldeia_mid.png`** — imagem-mestra da fase (ceu + cabanas + chao num unico PNG).
2. **`aldeia_fg.png`** — opcional, vinhas/troncos nas laterais com **centro transparente**.

O jogo usa so isso. O resultado fica igual ao PNG bonito que voce exportou.

### Opcao B — Parallax com 4 camadas (depois)

So funciona se **cada camada tiver transparencia** onde nao ha pixel daquele plano:

| Arquivo | Conteudo | Resto do PNG |
|---------|----------|--------------|
| `aldeia_sky.png` | Ceu, lua, fumaça distante | Transparente |
| `aldeia_mid.png` | Cabanas, arvores, fogo | Transparente em cima e embaixo |
| `aldeia_ground.png` | Chao **vista lateral** da faixa de andar | Transparente |
| `aldeia_fg.png` | Primeiro plano nas bordas | Centro transparente |

`aldeia_ground` atual e vista de cima — nao combina com beat 'em up lateral.
Regerar em vista lateral ou usar o chao que ja vem no `aldeia_mid`.

## Onde colocar arquivos

```text
assets/art/
  aldeia_mid.png      # imagem-mestra (obrigatorio)
  aldeia_fg.png       # primeiro plano (opcional)
  aldeia_sky.png      # reservado para Opcao B
  aldeia_ground.png   # reservado para Opcao B
  concept/
  sprites/player/
  sprites/enemies/
```

## Exportacao

- Formato: **PNG**
- Personagens: fundo **transparente**
- Fundo mestre: pode ser opaco (um arquivo so)
- Camadas parallax: **obrigatorio** transparencia entre planos
- Largura: **960px+** (ideal ~2172 para scroll)
- Filtro no Godot: **Linear** para pintura suave

Ver `docs/11_guia_krita_beat_em_up.md`.
