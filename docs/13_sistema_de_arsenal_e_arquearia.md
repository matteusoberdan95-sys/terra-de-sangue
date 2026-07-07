# Sistema de Arsenal, Arquearia e Artefatos

Ultima atualizacao: 2026-07-07.

## Visao

Arandu nao e um arqueiro — e um guerreiro de **tacape** que **usa o que a terra sangrada deixa para tras**: flechas caidas, ferro roubado, ossos talhados. O combate corpo a corpo continua sendo o coracao; arco e artefatos sao **ferramentas de pressao, bleed e abertura**, nao um segundo jogo dentro do jogo.

Inspiracao de sensacao: peso de **Soulslike** + leitura de **beat 'em up** — cada recurso tem limite, risco e consequencia visual.

---

## Pilares de design

1. **Tacape e lei** — arma principal, sempre equipada, evolui com o jogo.
2. **Flechas sao municao** — coletadas no chao, cap de aljava, nunca infinitas na fase.
3. **Sangramento e progressao** — flechas e certos golpes aplicam bleed; niveis escalam com upgrades e artefatos.
4. **Artefatos sao aposta** — poder alto, durabilidade baixa, quebram rapido.
5. **Gore com hierarquia** — bleed mancha; hemorragia deixa rastro; mutilacao e rara e contextual.
6. **Tudo conversa com Memoria da Terra** — upgrades permanentes vem de memorias; consumiveis vem do cenario.

---

## Arma principal: Tacape

### Papel

- Combos `J`/`K`, execucoes `E`, impulso e corrida.
- Esmagamento, stagger, knockback — direcao em `docs/11_direcao_de_violencia_gore.md`.

### Tiers de evolucao (meta-jogo)

| Tier | Nome ficticio | Efeito gameplay | Efeito gore |
|------|---------------|-----------------|-------------|
| 0 | Tacape de Ritual | Baseline atual | Sangue em impacto curto |
| 1 | Tacape de Osso e Couro | +1 dano leve, combo window +0.02s | Ferida visivel mais cedo |
| 2 | Tacape de Pedra-Rape | Pesado `K` aplica Sangramento I | Rachadura no torso inimigo |
| 3 | Tacape de Raiz Queimada | Contra-ataque e dash attack aplicam bleed | Decal de sangue maior |
| 4 | Tacape Ancestral | Execucoes desbloqueiam variante extra | Mutilacao em chefe ja ferido |

### Como desbloquear

- **Memorias da Terra** (permanente): fragmentos apos chefe ou pickup narrativo.
- **Nao** drop aleatorio por inimigo comum — evita farm sem peso.

### Na Fase 1 (prototipo)

- Implementar **Tier 0 → Tier 1** apenas; UI mostra slot de upgrade futuro.

---

## Arquearia: Arco e Aljava

### Fantasia

Arco leve de madeira queimada / corda de tendao — **nao e arma primaria**, e ferramenta de **kite, bleed e punir recuo**.

### Controles propostos

| Input | Acao |
|-------|------|
| `R` (segurar) | Entrar em mira — Arandu para, faixa de mira na direcao do `facing` |
| `R` (soltar) | Disparar 1 flecha (se aljava > 0) |
| `R` + `W`/`S` | Ajustar levemente eixo Y da mira (faixa 2.5D) |

> Alternativa futura: `R` tap rapido = disparo sem pausa longa (Tier 2 de arco).

### Aljava

| Parametro | Valor inicial |
|-----------|----------------|
| Capacidade max | 5 |
| Cap apos upgrades | 8 / 12 |
| Flechas ao iniciar fase | 0–2 (design de encontro) |
| Pickup no chao | +1 ou +3 (feixe raro) |

### Flechas no cenario

- Spawn em **pontos de director** (PhaseDirector), nao aleatorio infinito.
- Visual: feixe no chao, pixel art simples, brilho fraco.
- Som: coleta seca + estalo de couro na aljava.

### Tipos de flecha (escalonamento)

| Tipo | Onde aparece | Efeito |
|------|--------------|--------|
| Pedra | Fase 1 | Dano baixo + Sangramento I |
| Osso serrado | Mata Fechada | Sangramento II se alvo ja ferido |
| Ferro enferrujado | Acampamento de Ferro | Hemorragia I + reduz armadura leve |
| Raiz negra | Templo / memoria | Hemorragia II + rastro de sangue ao mover |

Fase 1 prototipo: **apenas Flecha de Pedra**.

---

## Sistema de Sangramento e Hemorragia

### Estados (em inimigos)

```
Integro → Sangramento I → Sangramento II → Hemorragia → Colapso
```

| Nivel | DPS | Duracao | Visual | Gameplay |
|-------|-----|---------|--------|----------|
| **Sangramento I** | 0.5/s | 4s | Mancha + `_bleedingWound` leve | Inimigo nao regenera |
| **Sangramento II** | 1/s | 5s | Gotas no chao ao andar | Telegraph +0.05s (mais legivel) |
| **Hemorragia I** | 1.5/s | 6s | Rastro no eixo Y | -10% velocidade |
| **Hemorragia II** | 2/s | 8s | Cor intensa, respiracao | Proximo pesado pode mutilar membro |
| **Colapso** | — | 2s janela | Pose fraca | Abre execucao ou critico garantido |

### Regras

- **Stack**: flecha aplica I; segundo hit de bleed sobe para II; hemorragia exige upgrade ou artefato.
- **Imunidade**: brutos resistem 1 nivel; chefe reduz duracao 50%.
- **Fair play**: bleed nao mata sozinho chefe — so deixa vulneravel.
- Integrar com `EnemyBase` e decals existentes em `ImpactFeedback`.

---

## Artefatos de fase (consumiveis)

### Conceito

Pecas encontradas **no meio da fase** — ferro da invasao, osso ritual, corrente quebrada. Arandu **equipa temporariamente** ou **carrega 1 uso** que substitui o proximo `K` ou ataque especial.

### Slots

| Slot | Comportamento |
|------|----------------|
| **Mao secundaria** | 1 artefato ativo por vez |
| **Durabilidade** | 2–5 usos ou 1 uso explosivo |
| **Quebra** | Animação curta + som de estilhaco; volta ao tacape |

### Catalogo inicial (Fase 1)

| Artefato | Usos | Ataque | Gore | Risco |
|----------|------|--------|------|-------|
| **Faca de Ferro Roubada** | 3 | Corte rapido frontal | Sangramento II + chance rasgar roupa | Alcance curto |
| **Gancho de Corrente** | 2 | Puxa inimigo na faixa Y | Sangramento I + stagger longo | Recovery alto |
| **Espinhos de Raiz** | 4 | Golpe leve amplo | DoT + slow | Dano base baixo |
| **Cabeca de Clava Quebrada** | 1 | Smash tipo `K` | Hemorragia instantanea | Quebra sempre apos uso |

### Spawn

- 1 artefato por sub-encontro da fase (PhaseDirector).
- Nunca obrigatorio para vencer — rota de estilo / velocidade.

### Narrativa

- Artefatos da invasao = **ironia**: usar ferro contra ferro.
- Artefatos de raiz = **custo espiritual** (futuro: barra de corrupcao leve).

---

## HUD e feedback

```
[Vida]
[Stamina]
[Aljava]  ●●●○○  (5 max)
[Artefato] Faca 2/3  (ou vazio)
[Tacape] Tier I
```

- Aljava pisca quando vazia e jogador segura `R`.
- Artefato mostra rachadura visual conforme durabilidade.
- Bleed nos inimigos: icone pequeno ou cor de silhueta (vermelho nas veias).

---

## Integracao com sistemas existentes

| Sistema | Integracao |
|---------|------------|
| `PlayerController` | Modo mira/disparo; troca artefato no `K` ou botao dedicado `U` |
| `EnemyBase` | Componente `BleedStatus`; expandir `_bleedingWound` |
| `CombatFeel` | Hitstop menor em flecha; maior em artefato que quebra |
| `CombatAudio` | SFX distintos: tension string, release, bleed tick |
| `PhaseDirector` | Spawn flechas e artefatos por encontro |
| `MemoryRegistry` | Tacape tier permanente |
| `CombatHud` | Aljava + artefato + tier tacape |

---

## Fora de escopo (por agora)

- Arco como arma infinita ou skill tree grande.
- Crafting entre fases.
- Multiplayer coop com divisao de flechas.
- Flechas guiadas ou sniper fora da faixa 2.5D.

---

## Criterio de sucesso (Goku)

O jogador deve sentir:

1. **Escassez** — "guardei 2 flechas para o bruto".
2. **Recompensa** — bleed deixa o chefe mais legivel e vulneravel.
3. **Aposta** — artefato quebra no momento certo ou no panic errado.
4. **Identidade** — tacape continua sendo "eu"; arco e faca sao o mundo falando.

Se arco virar spam a distancia segura, **reduzir cap, aumentar recovery pos-disparo ou stamina por flecha**.

---

## Roadmap de implementacao

| Sprint | Foco |
|--------|------|
| **18** | Pulo + combos 3 hits (movimento) |
| **19** | Arsenal core: aljava, pickup, disparo, bleed I |
| **20** | Tacape tier + 1 artefato consumivel |
| **21** | Hemorragia, tipos de flecha, polish gore bleed |

Detalhes da Sprint 19 em `docs/sprints/sprint_19_arsenal_arquearia.md`.
