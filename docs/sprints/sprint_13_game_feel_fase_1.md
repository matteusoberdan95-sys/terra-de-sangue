# Sprint 13 - Game Feel Fase 1

## Objetivo

Tornar o combate da Aldeia em Cinzas mais legivel, pesado e divertido sem arte nova.

## Entregas

- [x] `CombatFeel` centraliza hitstop, shake e stun por tipo de golpe.
- [x] Telegraph visual nos inimigos (marcador + recuo antes do ataque).
- [x] Hitstop diferenciado: leve, combo, pesado e execucao.
- [x] Knockback e hit stun escalados por golpe e dano.
- [x] SFX distintos para `J`, combo e `K`.
- [x] Encontros da fase 1 rebalanceados (menos spam, escalada mais clara).
- [x] Camera shake com decay suave apos hitpause.

## Validacao

- Build C# sem erros.
- Godot carrega combate com telegraphs e hitstop.

## Agentes

- Goku: telegraph e ritmo dos encontros.
- Krillin: regressao do fluxo da fase 1.
- Vegeta: `CombatFeel` sem acoplamento excessivo.

## Registro de validacao visual

Status atual: validado localmente no Godot em 2026-07-07. Aprovado pelo usuario.

Resultado: aprovado pelo usuario em 2026-07-07.

Cena para validar:

```text
scenes/Main.tscn
```

Checklist de teste interativo:

- Inimigos avisam antes de atacar (marcador laranja + recuo).
- `J`, combo e `K` soam e sentem diferentes.
- Golpe pesado e execucao tem mais peso (hitstop + shake).
- Bruto e sargento parecem perigosos mas justos.
- 3 encontros escalam sem parecer caotico.
