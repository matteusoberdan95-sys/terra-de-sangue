# Sprint 15 - Sprites Pixel Fase 1

## Objetivo

Substituir poligonos do Arandu e mercenario por sprites pixel animados, mantendo combate e som validados.

## Entregas

- [x] `AranduSpriteArt` com idle, walk, attack_light, attack_heavy e hit.
- [x] `MercenarySpriteArt` com idle, walk, attack e hit.
- [x] `SpriteCharacterAnimator` integrado ao `VisualRig`.
- [x] `AudioLibrary` com fallback para `PlaceholderSfx` e pastas em `assets/audio/`.
- [x] Estrutura `assets/art/sprites/` pronta para arte final.

## Validacao

- Build C# sem erros.
- Godot exibe sprites animados na fase 1.
- Validacao interativa pendente para o usuario.

## Registro de validacao visual

Status atual: pendente.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist:

- Arandu anima ao andar e atacar.
- Mercenarios usam sprite pixel (nao poligono).
- Brutos e sargento ainda em silhueta (proxima iteracao).
- Combate e som preservados.
