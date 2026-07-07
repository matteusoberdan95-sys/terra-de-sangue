# Sprites do Arandu

## Atual

| Arquivo | Uso | Frames |
|---------|-----|--------|
| `arandu_idle_sheet.png` | idle vivo inicial do Arandu | 8 frames, 256x256 |
| `arandu_walk_sheet.png` | caminhada inicial do Arandu | 8 frames, 256x256 |
| `arandu_attack_light_sheet.png` | ataque leve inicial do Arandu | 8 frames, 256x256 |
| `arandu_attack_heavy_sheet.png` | ataque forte inicial do Arandu | 8 frames, 256x256 |

## Regras

- Manter fundo transparente.
- Manter 8 frames em linha horizontal para ciclos base.
- Usar estes sheets aprovados como referencia de paleta, proporcao, cabelo, arma, tecido e postura.
- Nao gerar run/ataque com outro corpo ou outra paleta.
- Se o tamanho do frame mudar, atualizar `AranduSpriteArt.cs`.
- Sheets recebidos com canvas irregular devem ser normalizados para 8 frames de 256x256 antes da integracao.
- Nao usar crop fixo por largura quando o canvas vier irregular; recortar por silhueta e revisar visualmente para nao cortar tacape/bracos.
- O scale externo atual do Arandu fica em `PlayerController.AttachAranduSprite()`; nao mudar isso para "corrigir" um sheet isolado.

## Proximos sheets

1. `arandu_run_sheet.png`
2. `arandu_hit_sheet.png`
3. `arandu_death_sheet.png`
