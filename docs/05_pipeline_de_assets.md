# Pipeline de Assets

## Filosofia

Assets entram no jogo por utilidade de sprint. Nao vamos gerar vinte personagens antes do combate estar provado.

## Ordem correta

1. Placeholder funcional.
2. Silhueta aprovada.
3. Arte conceitual.
4. Sprite base.
5. Animacao minima.
6. Integracao no Godot.
7. Validacao visual.
8. Polimento.

## Tamanhos iniciais

- Resolucao base interna: `480x270`.
- Zoom de camera inicial: `2x`.
- Player em tela: aproximadamente `48px` a `64px` de altura na arte final.
- Inimigos comuns: `44px` a `70px`.
- Chefes: `90px` a `140px`.

## Pastas

- `assets/art/concept/`: conceitos gerados.
- `assets/art/sprites/player/`: sprites do protagonista.
- `assets/art/sprites/enemies/`: sprites inimigos.
- `assets/art/backgrounds/`: fundos e camadas.
- `assets/audio/`: som e musica.
- `assets/fonts/`: fontes.
- `assets/materials/`: shaders e materiais.

## Prompt checklist

Todo prompt de imagem deve declarar:

- Funcao do asset.
- Vista lateral ou sprite sheet.
- Silhueta.
- Paleta.
- Iluminacao.
- Fundo transparente quando for sprite.
- Proibicao de texto embutido na imagem.
- Consistencia com Terra Sangrada.
