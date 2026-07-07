# Sprint 5 - Polimento de Impacto

## Objetivo

Deixar o combate mais visceral com sangue temporario, feedback de morte, feridas progressivas, decals no chao e sons placeholder.

## Escopo

Entra:

- Refino do hit pause e shake ja existentes.
- Sangue temporario em splatters.
- Decals de sangue no chao com fade.
- Sons placeholder para hit, morte e dano no player.
- Feedback visual de morte do inimigo.
- Primeiro sistema de feridas/roupa rasgada por nivel de vida.

Nao entra:

- Gore persistente permanente.
- Desmembramento.
- Execucoes.
- Arte final de particulas.
- Audio final.

## Checklist tecnico

- [x] Cena `Main.tscn` continua abrindo.
- [x] Build .NET 10 sem erros.
- [x] Godot console sem erro de assembly/script.
- [x] `ImpactFeedback` centraliza efeitos.
- [x] `BloodSplatter` e `BloodDecal` adicionados.
- [x] `PlaceholderSfx` gera sons procedurais.
- [x] Inimigos mostram feridas progressivas.
- [x] Hit pause e shake preservados.

## Checklist de sensacao

- [x] Golpes geram sangue visivel sem esconder a acao.
- [x] Chao acumula manchas temporarias.
- [x] Morte do inimigo parece mais brutal.
- [x] Inimigos feridos parecem piores conforme perdem vida.
- [x] Sons placeholder reforcam impacto.
- [x] Validacao visual humana no Godot.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Resultado: aprovado pelo usuario em 2026-07-07.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste interativo:

- `J` gera splatter e decal de sangue.
- Inimigos mostram roupa rasgada e ferida ao perder vida.
- Morte inclui som, sangue extra e corpo tombando.
- Player ferido dispara feedback de sangue/som.
- Decals somem com o tempo sem poluir demais a faixa jogavel.

## Agentes envolvidos

- Goku valida peso e visceralidade do combate.
- Piccolo valida leitura do sangue e feridas.
- Vegeta valida arquitetura de `ImpactFeedback` e build.
- Krillin valida regressao jogavel.
- Trunks atualiza estado e proximo passo.
