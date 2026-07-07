# Sprint 14 - Audio Fase 1

## Objetivo

Dar camada sonora completa ao combate da Aldeia em Cinzas, alinhada ao game feel validado.

## Entregas

- [x] `CombatAudio` centraliza todos os sons de combate e UI da fase.
- [x] Impactos diferenciados: leve, combo, pesado e morte.
- [x] Dano no player leve vs pesado.
- [x] Telegraph sonoro e swing dos inimigos.
- [x] Execucoes contextuais com sons distintos.
- [x] Pulso nos encontros 2 e 3, intro do mini-chefe.
- [x] Som ao coletar memoria.

## Validacao

- Build C# sem erros.
- Godot carrega `CombatAudio` na arena.
- Validacao interativa pendente para o usuario.

## Agentes

- Goku: sons reforcam telegraph e peso dos golpes.
- Bulma: `CombatAudio` desacoplado do feedback visual.
- Krillin: regressao do combate e fluxo da fase 1.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Resultado: aprovado pelo usuario em 2026-07-07. Placeholders procedurais mantidos ate sprint de SFX custom.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste interativo:

- Inimigos emitem aviso sonoro antes de atacar.
- `J`, combo e `K` soam distintos ao swing e ao impacto.
- Dano do bruto/sargento soa mais pesado no player.
- Mini-chefe tem entrada sonora.
- Memoria emite som ao coletar.
- Audio nao polui nem repete de forma irritante.
